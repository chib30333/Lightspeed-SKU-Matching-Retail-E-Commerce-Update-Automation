using DocumentFormat.OpenXml.InkML;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Dupont_Price_Lists.Output;
using Dupont_Price_Lists.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services
{
    public static class OnlineBuilder
    {
        public static Dictionary<string, ItemRecord> BuildOnlineIndex(
        IEnumerable<ItemRecord> onlineData,
        string onlineSkuField,
        Func<string?, string?> keyNormalizer)
        {
            var idx = new Dictionary<string, ItemRecord>(StringComparer.OrdinalIgnoreCase);

            foreach (var rec in onlineData)
            {
                var raw = rec.GetField(onlineSkuField ?? "");
                var key = keyNormalizer(raw);
                if (string.IsNullOrEmpty(key)) continue;

                // If duplicates exist in B, prefer the first or overwrite—your call.
                if (!idx.ContainsKey(key))
                    idx[key] = rec;
            }
            return idx;
        }

        public static List<ItemRecord> BuildAndValidateFileE(
            MatchResult match,
            IEnumerable<ItemRecord> onlineData,
            FieldMapping map,
            Func<string?, string?> keyNormalizer,
            DiscountResolver discountResolver,
            out OnlineReport report)
        {
            var OnlineRows = OnlineBuilder.BuildOnline(match, onlineData, map, keyNormalizer, discountResolver);

            DeduplicateOnline(OnlineRows);

            report = ValidateOnline(OnlineRows);

            return OnlineRows;
        }
        public static string Get(ItemRecord r, string? field) => string.IsNullOrWhiteSpace(field) ? "" : r.GetField(field);

        public static List<ItemRecord> BuildOnline(
        MatchResult match,
        IEnumerable<ItemRecord> onlineData,       // File B
        FieldMapping map,
        Func<string?, string?> keyNormalizer,     // your normalizer; allow nullable return
        DiscountResolver discountResolver // e.g., chosen pricing profile => "50/10"
        )
        {
            var OnlineRows = new List<ItemRecord>();
            if (string.IsNullOrWhiteSpace(map.LightspeedEcomField))
                return OnlineRows; // nothing to do safely

            // Index File B by SKU (normalized)
            var onlineIdx = BuildOnlineIndex(onlineData, OnlineFields.ManufactSku ?? map.VendorSkuField, keyNormalizer);

            foreach (var fm in match.Found)
            {
                var ls = fm.LightspeedPrimary;
                var vendor = fm.Vendor;

                // Include only if Ecom = Y in A
                var ecomFlag = Get(ls, map.LightspeedEcomField).Trim();
                if (!ecomFlag.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    continue;

                var skuRaw = Get(vendor, map.VendorSkuField);
                var skuKey = keyNormalizer(skuRaw);
                onlineIdx.TryGetValue(skuKey ?? "", out var onlineRec); // may be null if not present in B

                var row = new ItemRecord();

                // Identity
                row.SetField(OnlineFields.SystemId, Get(ls, map.LightspeedSystemIdField));
                row.SetField(OnlineFields.ManufactSku, skuRaw);
                row.SetField(OnlineFields.EcomFlag, "Y");

                // Descriptive context (optional but helpful)
                var brand = Get(ls, OnlineFields.Brand);
                if (string.IsNullOrWhiteSpace(brand)) brand = Get(vendor, map.VendorBrandField);
                row.SetField(OnlineFields.Brand, brand);

                var category = Get(ls, OnlineFields.Category);
                if (string.IsNullOrWhiteSpace(category)) category = Get(vendor, OnlineFields.Category);
                row.SetField(OnlineFields.Category, category);

                var desc = Get(ls, map.VendorDescriptionField);
                if (string.IsNullOrWhiteSpace(desc)) desc = Get(vendor, map.VendorDescriptionField);
                row.SetField(OnlineFields.Description, desc);

                // Online specifics from File B (if present)
                if (onlineRec != null)
                {
                    row.SetField(OnlineFields.VariantId, Get(onlineRec, OnlineFields.VariantId));
                    row.SetField(OnlineFields.ShippingWeight, Get(onlineRec, OnlineFields.ShippingWeight));
                    row.SetField(OnlineFields.BoxDimA, Get(onlineRec, OnlineFields.BoxDimA));
                    row.SetField(OnlineFields.BoxDimB, Get(onlineRec, OnlineFields.BoxDimB));
                    row.SetField(OnlineFields.BoxDimC, Get(onlineRec, OnlineFields.BoxDimC));
                }
                else
                {
                    // If no match in File B, keep blanks—importer may tolerate or you can flag later.
                    row.SetField(OnlineFields.VariantId, "");
                    row.SetField(OnlineFields.ShippingWeight, "");
                    row.SetField(OnlineFields.BoxDimA, "");
                    row.SetField(OnlineFields.BoxDimB, "");
                    row.SetField(OnlineFields.BoxDimC, "");
                }

                // Online Price (reuse same discount rule strategy as File D)
                var msrp = PriceMath.ParseDecimal(vendor.GetField(map.LightspeedMsrpField));
                if (msrp == 0m) msrp = PriceMath.ParseDecimal(ls.GetField(map.LightspeedMsrpField));

                var rules = discountResolver.ResolveRule(
                    vendor,
                    brandField: map.VendorBrandField,
                    skuField: map.VendorSkuField,
                    customSkuField: map.LightspeedCustomSkuField,
                    map.VendorDescriptionField, map.VendorBrandField
                ) ?? new Dictionary<string, string>();

                foreach (var ruleItem in rules)
                {
                    if (ruleItem.Key.Contains("Online"))
                    {
                        row.SetField(OnlineFields.OnlinePrice, msrp.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    }
                }

                OnlineRows.Add(row);
            }

            return OnlineRows;
        }

        public static void DeduplicateOnline(List<ItemRecord> OnlineRows)
        {
            var groups = OnlineRows
                .GroupBy(r => r.GetField(OnlineFields.ManufactSku), StringComparer.OrdinalIgnoreCase);
            var keep = new List<ItemRecord>();

            foreach (var g in groups)
            {
                var list = g.ToList();

                // Prefer row with VariantId (if any). If multiple, pick highest OnlinePrice.
                var prioritized = list
                    .OrderByDescending(r => !string.IsNullOrWhiteSpace(r.GetField(OnlineFields.VariantId)))
                    .ThenByDescending(r =>
                    {
                        var s = r.GetField(OnlineFields.OnlinePrice);
                        return decimal.TryParse(s, out var d) ? d : 0m;
                    })
                    .ToList();

                keep.Add(prioritized.First());
            }

            OnlineRows.Clear();
            OnlineRows.AddRange(keep);
        }

        public class OnlineReport
        {
            public int ConsideredFound { get; set; }
            public int EcomY { get; set; }
            public int MissingVariant { get; set; }
            public int OutputRows { get; set; }
            public List<string> Warnings { get; } = new();
        }

        public static OnlineReport ValidateOnline(List<ItemRecord> OnlineRows)
        {
            var report = new OnlineReport { OutputRows = OnlineRows.Count };

            foreach (var r in OnlineRows)
            {
                if (string.IsNullOrWhiteSpace(r.GetField(OnlineFields.ManufactSku)))
                    report.Warnings.Add("Row missing Manufacturer SKU.");

                if (!"Y".Equals(r.GetField(OnlineFields.EcomFlag), StringComparison.OrdinalIgnoreCase))
                    report.Warnings.Add($"SKU {r.GetField(OnlineFields.ManufactSku)} has Ecom != Y.");

                if (string.IsNullOrWhiteSpace(r.GetField(OnlineFields.VariantId)))
                    report.MissingVariant++;
            }
            return report;
        }

    }
}
