using System;
using System.Collections.Generic;
using System.Linq;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Matching;
using Dupont_Price_Lists.Models.Outputs;
using Dupont_Price_Lists.Services.Categories;
using Dupont_Price_Lists.Services.Discounts;
using Dupont_Price_Lists.Services.Pricing;

namespace Dupont_Price_Lists.Services.Builders
{
    public sealed class RetailBuilder
    {
        public List<RetailRow> Build(
            MatchResult match,
            MappingProfile profile,
            ICategoryEngine categoryEngine,
            IReadOnlyList<DiscountRule> discountRules)
        {
            var rows = new List<RetailRow>(match.Retail.Count);

            foreach (var m in match.Retail)
            {
                var vendor = m.VendorItem;
                var ls = m.LightspeedItem;

                string GetFieldSafe(ItemRecord r, string? field)
                    => string.IsNullOrWhiteSpace(field) ? "" : r.GetField(field);

                var sku = GetFieldSafe(vendor, profile.VendorSkuField);
                if (string.IsNullOrWhiteSpace(sku)) continue;

                var brand = ResolveBrand(profile, vendor, ls);

                var vendorName = profile.UseFixedVendor
                    ? (profile.FixedVendor ?? "")
                    : GetFieldSafe(ls ?? new ItemRecord(), profile.VendorVendorField);

                var customSku = ls?.GetField(profile.LightspeedCustomSkuField) ?? "";
                var upc = ls?.GetField(profile.LightspeedUpcField) ?? GetFieldSafe(vendor, profile.VendorUpcField);

                var finish = GetFieldSafe(vendor, profile.VendorFinishField);
                var desc = GetFieldSafe(ls ?? new ItemRecord(), "Item") ?? GetFieldSafe(vendor, profile.VendorDescriptionField);

                var msrp = ResolveMsrp(vendor, ls, profile);

                var finalDesc = BuildDescription(profile.NewDescriptionTemplate, brand, desc, finish, sku);
                finalDesc = EnsureBrandDash(brand, finalDesc);

                if (m.RecordType == "Found" && !string.IsNullOrWhiteSpace(desc))
                    finalDesc = desc;

                var scanFields = (profile.CategoryScanFields != null && profile.CategoryScanFields.Count > 0)
                    ? profile.CategoryScanFields
                    : new List<string> { profile.VendorDescriptionField ?? "Description" };

                var cat = categoryEngine.Resolve(vendor, scanFields) ?? "";

                // -----------------------------
                // DISCOUNT LOOKUP KEYS (Brand + Vendor)
                // -----------------------------
                var keys = new List<string>();

                var brandKey = Services.Matching.MatchRetailOptions.DefaultNormalizer(brand);
                if (!string.IsNullOrWhiteSpace(brandKey))
                    keys.Add(brandKey);

                var vendorKey = Services.Matching.MatchRetailOptions.DefaultNormalizer(vendorName);
                if (!string.IsNullOrWhiteSpace(vendorKey) && !keys.Contains(vendorKey, StringComparer.OrdinalIgnoreCase))
                    keys.Add(vendorKey);

                var hay = $"{desc} {finish} {sku} {brand} {vendorName} {customSku}".ToLowerInvariant();

                var rule = DiscountRuleSelector.Select(
                    discountRules,
                    lookupKeys: keys,
                    sku: sku,
                    customSku: customSku,
                    haystackLower: hay
                );

                var rr = new RetailRow
                {
                    RecordType = m.RecordType,
                    SystemId = ls?.GetField(profile.LightspeedSystemIdField),
                    ManufactSku = sku,
                    CustomSku = customSku,
                    Upc = upc,

                    Brand = brand,
                    Vendor = vendorName,

                    Finish = finish,
                    Category = cat,
                    Description = finalDesc,

                    Msrp = msrp
                };

                rr.DefaultCost = DiscountEngine.Apply(msrp, rule?.DefaultCostRule);
                rr.VendorCost = DiscountEngine.Apply(msrp, rule?.VendorCostRule);
                rr.DefaultPrice = DiscountEngine.Apply(msrp, rule?.DefaultPriceRule);
                rr.RetailPrice = DiscountEngine.Apply(msrp, rule?.RetailPriceRule);
                rr.ContractorPrice = DiscountEngine.Apply(msrp, rule?.ContractorPriceRule);
                rr.DesignerPrice = DiscountEngine.Apply(msrp, rule?.DesignerPriceRule);
                rr.OnlinePrice = DiscountEngine.Apply(msrp, rule?.OnlinePriceRule);
                rr.VipPrice = DiscountEngine.Apply(msrp, rule?.VipPriceRule);

                rr.Archive = false;
                rows.Add(rr);
            }

            ArchiveLowerMsrpDuplicates(rows);
            return rows;
        }

        private static string? ResolveBrand(MappingProfile profile, ItemRecord vendor, ItemRecord? ls)
        {
            if (profile.UseFixedBrand && !string.IsNullOrWhiteSpace(profile.FixedBrand))
                return profile.FixedBrand;

            if (profile.UseBrandFromField && !string.IsNullOrWhiteSpace(profile.VendorBrandField))
            {
                var b = vendor.GetField(profile.VendorBrandField);
                if (!string.IsNullOrWhiteSpace(b)) return b;
            }

            var lsBrand = ls?.GetField("Brand");
            return string.IsNullOrWhiteSpace(lsBrand) ? "" : lsBrand;
        }

        private static decimal ResolveMsrp(ItemRecord vendor, ItemRecord? ls, MappingProfile profile)
        {
            if (!string.IsNullOrWhiteSpace(profile.VendorPriceField))
            {
                var v = vendor.GetField(profile.VendorPriceField);
                var d = PriceMath.ParseDecimal(v);
                if (d > 0m) return d;
            }

            if (ls != null)
            {
                var d = PriceMath.ParseDecimal(ls.GetField(profile.LightspeedMsrpField));
                if (d > 0m) return d;
            }

            return 0m;
        }

        private static string BuildDescription(string tmpl, string? brand, string? desc, string? finish, string? sku)
        {
            string S(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();
            return (tmpl ?? "{BRAND} - {DESC} - {FINISH} - {SKU}")
                .Replace("{BRAND}", S(brand))
                .Replace("{DESC}", S(desc))
                .Replace("{FINISH}", S(finish))
                .Replace("{SKU}", S(sku))
                .Trim();
        }

        private static string EnsureBrandDash(string? brand, string description)
        {
            var b = (brand ?? "").Trim();
            var d = (description ?? "").Trim();
            if (string.IsNullOrEmpty(b)) return d;
            if (d.StartsWith(b + " - ", StringComparison.OrdinalIgnoreCase)) return d;

            if (d.StartsWith(b + " ", StringComparison.OrdinalIgnoreCase))
                d = d.Substring(b.Length).TrimStart('-', ' ');

            return $"{b} - {d}";
        }

        private static void ArchiveLowerMsrpDuplicates(List<RetailRow> rows)
        {
            var groups = rows.GroupBy(r => (r.ManufactSku ?? "").Trim(), StringComparer.OrdinalIgnoreCase);

            foreach (var g in groups)
            {
                var list = g.ToList();
                if (list.Count <= 1) continue;

                var ordered = list.OrderByDescending(x => x.Msrp).ToList();

                bool first = true;
                foreach (var x in ordered)
                {
                    x.Archive = !first;
                    first = false;
                }
            }
        }
    }
}
