using System;
using System.Collections.Generic;
using System.Linq;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Matching;
using Dupont_Price_Lists.Models.Outputs;
using Dupont_Price_Lists.Services.Discounts;
using Dupont_Price_Lists.Services.Matching;
using Dupont_Price_Lists.Services.Pricing;

namespace Dupont_Price_Lists.Services.Builders
{
    public sealed class OnlineBuilder
    {
        public List<OnlineRow> Build(
            MatchResult match,
            List<ItemRecord> onlineFileRows, // File B
            MappingProfile profile,
            IReadOnlyList<DiscountRule> discountRules)
        {
            string GetSafe(ItemRecord r, string? field)
                => string.IsNullOrWhiteSpace(field) ? "" : r.GetField(field);

            // Normalizer
            var norm = MatchRetailOptions.DefaultNormalizer;

            // Index File B by SKU (normalized)
            var onlineIndex = new Dictionary<string, ItemRecord>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in onlineFileRows)
            {
                var rawSku = GetSafe(r, profile.OnlineSkuField);
                var key = norm(rawSku) ?? "";
                if (string.IsNullOrEmpty(key)) continue;

                // keep first
                if (!onlineIndex.ContainsKey(key))
                    onlineIndex[key] = r;
            }

            var outRows = new List<OnlineRow>();

            foreach (var fm in match.Found)
            {
                var ls = fm.LightspeedPrimary;
                var vendor = fm.Vendor;

                // Only include LS items where EcomFlag == "Y"
                var ecomFlag = ls.GetField(profile.LightspeedEcomField).Trim();
                if (!ecomFlag.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    continue;

                // SKU comes from vendor SKU field (your matching anchor)
                var sku = GetSafe(vendor, profile.VendorSkuField);
                if (string.IsNullOrWhiteSpace(sku))
                    continue;

                var skuKey = norm(sku) ?? "";

                // Find File B online record (optional)
                onlineIndex.TryGetValue(skuKey, out var onlineRec);

                // Brand: fixed brand > vendor mapped brand field > LS Brand
                var brand = ResolveBrand(profile, vendor, ls);

                // Vendor name (for rule fallback): fixed vendor > LS Vendor (if exists)
                var vendorName = profile.UseFixedVendor
                    ? (profile.FixedVendor ?? "")
                    : ls.GetField("Vendor"); // if your LS export uses another header, change here

                // Descriptive fields
                var desc = GetSafe(vendor, profile.VendorDescriptionField);
                if (string.IsNullOrWhiteSpace(desc))
                    desc = ls.GetField("Description");

                var category = ls.GetField("Category");
                var customSku = ls.GetField(profile.LightspeedCustomSkuField);

                // MSRP: prefer vendor mapped price (if mapped), else LS MSRP
                decimal msrp = 0m;
                if (!string.IsNullOrWhiteSpace(profile.VendorPriceField))
                    msrp = PriceMath.ParseDecimal(GetSafe(vendor, profile.VendorPriceField));
                if (msrp <= 0m)
                    msrp = PriceMath.ParseDecimal(ls.GetField(profile.LightspeedMsrpField));

                // -----------------------------
                // Discount lookup keys: Brand + Vendor
                // -----------------------------
                var keys = new List<string>();

                var brandKey = norm(brand);
                if (!string.IsNullOrWhiteSpace(brandKey))
                    keys.Add(brandKey);

                var vendorKey = norm(vendorName);
                if (!string.IsNullOrWhiteSpace(vendorKey) &&
                    !keys.Contains(vendorKey, StringComparer.OrdinalIgnoreCase))
                    keys.Add(vendorKey);

                // Haystack for TagContains rules (include customSku, brand, vendor, desc)
                var hay = $"{desc} {sku} {brand} {vendorName} {customSku}".ToLowerInvariant();

                var rule = DiscountRuleSelector.Select(
                    discountRules,
                    lookupKeys: keys,
                    sku: sku,
                    customSku: customSku ?? "",
                    haystackLower: hay
                );

                // Build Online row
                var row = new OnlineRow
                {
                    SystemId = ls.GetField(profile.LightspeedSystemIdField),
                    ManufactSku = sku,
                    EcomFlag = "Y",

                    Brand = brand,
                    Description = desc,
                    Category = category
                };

                // Fill from File B when available
                if (onlineRec != null)
                {
                    row.VariantId = onlineRec.GetField("Variant ID");
                    row.ShippingWeight = onlineRec.GetField("Shipping Weight");
                    row.BoxDimA = onlineRec.GetField("Shipping Box Dimensions A");
                    row.BoxDimB = onlineRec.GetField("Shipping Box Dimensions B");
                    row.BoxDimC = onlineRec.GetField("Shipping Box Dimensions C");
                }
                else
                {
                    row.VariantId = "";
                    row.ShippingWeight = "";
                    row.BoxDimA = "";
                    row.BoxDimB = "";
                    row.BoxDimC = "";
                }

                // Apply Online price rule correctly
                row.OnlinePrice = DiscountEngine.Apply(msrp, rule?.OnlinePriceRule);

                outRows.Add(row);
            }

            // Deduplicate by SKU: prefer VariantId, then highest OnlinePrice
            var deduped = outRows
                .GroupBy(r => r.ManufactSku ?? "", StringComparer.OrdinalIgnoreCase)
                .Select(g => g
                    .OrderByDescending(x => !string.IsNullOrWhiteSpace(x.VariantId))
                    .ThenByDescending(x => x.OnlinePrice)
                    .First())
                .ToList();

            return deduped;
        }

        private static string ResolveBrand(MappingProfile profile, ItemRecord vendor, ItemRecord ls)
        {
            if (profile.UseFixedBrand && !string.IsNullOrWhiteSpace(profile.FixedBrand))
                return profile.FixedBrand!;

            if (profile.UseBrandFromField && !string.IsNullOrWhiteSpace(profile.VendorBrandField))
            {
                var b = vendor.GetField(profile.VendorBrandField);
                if (!string.IsNullOrWhiteSpace(b)) return b;
            }

            // fallback to LS Brand
            return ls.GetField("Brand") ?? "";
        }
    }
}
