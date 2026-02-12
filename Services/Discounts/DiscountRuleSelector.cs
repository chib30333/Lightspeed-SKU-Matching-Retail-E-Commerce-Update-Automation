using System;
using System.Collections.Generic;
using System.Linq;

namespace Dupont_Price_Lists.Services.Discounts
{
    public static class DiscountRuleSelector
    {
        public static DiscountRule? Select(
            IEnumerable<DiscountRule> rules,
            string brandKey,
            string sku,
            string customSku,
            string haystackLower)
        {
            var brandRules = rules.Where(r => r.BrandKey.Equals(brandKey, StringComparison.OrdinalIgnoreCase)).ToList();
            if (brandRules.Count == 0) return null;

            // Apply optional filters first (TagContains and SkuStartsWith)
            var filtered = brandRules.Where(r =>
            {
                bool tagOk = string.IsNullOrWhiteSpace(r.TagContains)
                    || haystackLower.Contains(r.TagContains.Trim().ToLowerInvariant())
                    || (customSku ?? "").ToLowerInvariant().Contains(r.TagContains.Trim().ToLowerInvariant());

                bool skuOk = string.IsNullOrWhiteSpace(r.SkuStartsWith)
                    || sku.StartsWith(r.SkuStartsWith.Trim(), StringComparison.OrdinalIgnoreCase);

                return tagOk && skuOk;
            }).ToList();

            // If any filtered rules exist, take first (you can later add priority column)
            if (filtered.Count > 0) return filtered[0];

            // fallback to brand-only rule (first)
            return brandRules[0];
        }
    }
}
