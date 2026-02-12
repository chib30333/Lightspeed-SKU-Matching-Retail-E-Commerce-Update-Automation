using System;
using System.Collections.Generic;
using System.Linq;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Matching;
using Dupont_Price_Lists.Models.Outputs;
using Dupont_Price_Lists.Services.Discounts;
using Dupont_Price_Lists.Services.Pricing;
using Dupont_Price_Lists.Services.Matching;

namespace Dupont_Price_Lists.Services.Builders
{
    public sealed class OnlineBuilder2
    {
        public List<OnlineRow> Build(
            MatchResult match,
            List<ItemRecord> onlineFileRows, // File B
            MappingProfile profile,
            IReadOnlyList<DiscountRule> discountRules)
        {
            var normalizer = MatchRetailOptions.DefaultNormalizer;

            // index File B by SKU
            var onlineIndex = new Dictionary<string, ItemRecord>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in onlineFileRows)
            {
                var key = normalizer(r.GetField(profile.OnlineSkuField)) ?? "";
                if (string.IsNullOrEmpty(key)) continue;
                if (!onlineIndex.ContainsKey(key))
                    onlineIndex[key] = r;
            }

            var outRows = new List<OnlineRow>();

            foreach (var fm in match.Found)
            {
                var ls = fm.LightspeedPrimary;
                var vendor = fm.Vendor;

                var ecomFlag = ls.GetField(profile.LightspeedEcomField).Trim();
                if (!ecomFlag.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    continue;

                var sku = vendor.GetField(profile.VendorSkuField);
                var skuKey = normalizer(sku) ?? "";

                onlineIndex.TryGetValue(skuKey, out var onlineRec);

                var brand = ls.GetField("Brand");
                if (string.IsNullOrWhiteSpace(brand) && profile.UseFixedBrand)
                    brand = profile.FixedBrand ?? "";

                var desc = vendor.GetField(profile.VendorDescriptionField ?? "");
                if (string.IsNullOrWhiteSpace(desc)) desc = ls.GetField("Description");

                var category = ls.GetField("Category");

                var customSku = ls.GetField(profile.LightspeedCustomSkuField) ?? "";
                var msrp = PriceMath.ParseDecimal(vendor.GetField(profile.VendorPriceField ?? ""));
                if (msrp <= 0m) msrp = PriceMath.ParseDecimal(ls.GetField(profile.LightspeedMsrpField));

                var brandKey = normalizer(brand) ?? "";
                var hay = $"{desc} {sku} {brand} {customSku}".ToLowerInvariant();
                var rule = DiscountRuleSelector.Select(discountRules, brandKey, sku, customSku, hay);

                var row = new OnlineRow
                {
                    SystemId = ls.GetField(profile.LightspeedSystemIdField),
                    ManufactSku = sku,
                    EcomFlag = "Y",
                    Brand = brand,
                    Description = desc,
                    Category = category
                };

                // Pull online specifics from File B
                if (onlineRec != null)
                {
                    row.VariantId = onlineRec.GetField("Variant ID");
                    row.ShippingWeight = onlineRec.GetField("Shipping Weight");
                    row.BoxDimA = onlineRec.GetField("Shipping Box Dimensions A");
                    row.BoxDimB = onlineRec.GetField("Shipping Box Dimensions B");
                    row.BoxDimC = onlineRec.GetField("Shipping Box Dimensions C");
                }

                // Apply OnlinePrice rule correctly (no MSRP-only bug)
                row.OnlinePrice = DiscountEngine.Apply(msrp, rule?.OnlinePriceRule);

                outRows.Add(row);
            }

            // Deduplicate by SKU: prefer VariantId, then highest OnlinePrice
            outRows = outRows
                .GroupBy(r => r.ManufactSku, StringComparer.OrdinalIgnoreCase)
                .Select(g => g
                    .OrderByDescending(x => !string.IsNullOrWhiteSpace(x.VariantId))
                    .ThenByDescending(x => x.OnlinePrice)
                    .First())
                .ToList();

            return outRows;
        }
    }
}
