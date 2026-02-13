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

                string GetVendor(ItemRecord r, string? field)
                    => string.IsNullOrWhiteSpace(field) ? "" : r.GetField(field);

                var sku = GetVendor(vendor, profile.VendorSkuField);
                if (string.IsNullOrWhiteSpace(sku)) continue;

                var brand = ResolveBrand(profile, vendor, ls);
                var vendorName = profile.UseFixedVendor ? profile.FixedVendor : null;
                var customSku = ls?.GetField(profile.LightspeedCustomSkuField) ?? "";
                var upc = ls?.GetField(profile.LightspeedUpcField) ?? vendor.GetField(profile.VendorUpcField ?? "");

                var finish = GetVendor(vendor, profile.VendorFinishField);
                var desc = GetVendor(vendor, profile.VendorDescriptionField);

                var msrp = ResolveMsrp(vendor, ls, profile);

                var finalDesc = BuildDescription(profile.NewDescriptionTemplate, brand, desc, finish, sku);
                finalDesc = EnsureBrandDash(brand, finalDesc);

                if (m.RecordType == "Found" && ls != null) finalDesc = desc;

                // Category resolution: scan configured fields
                var scanFields = profile.CategoryScanFields?.Count > 0 ? profile.CategoryScanFields : new List<string> { profile.VendorDescriptionField ?? "DESCRIPTION" };
                var cat = categoryEngine.Resolve(vendor, scanFields) ?? "";

                var brandKey = Services.Matching.MatchRetailOptions.DefaultNormalizer(brand) ?? "";
                var hay = $"{desc} {finish} {sku} {brand} {customSku}".ToLowerInvariant();

                var rule = DiscountRuleSelector.Select(discountRules, brandKey, sku, customSku, hay);

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

                // Apply rules (no hardcoding)
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

            // fallback to lightspeed if it has Brand column
            var lsBrand = ls?.GetField("Brand");
            return string.IsNullOrWhiteSpace(lsBrand) ? "" : lsBrand;
        }

        private static decimal ResolveMsrp(ItemRecord vendor, ItemRecord? ls, MappingProfile profile)
        {
            // Prefer vendor "price" or "msrp" column if mapped, else LS MSRP
            if (!string.IsNullOrWhiteSpace(profile.VendorPriceField))
            {
                var v = vendor.GetField(profile.VendorPriceField);
                var d = PriceMath.ParseDecimal(v);
                if (d > 0m) return d;
            }

            // fallback to LS MSRP
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

                // keep highest MSRP as active
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
