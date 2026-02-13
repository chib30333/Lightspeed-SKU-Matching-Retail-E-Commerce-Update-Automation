using System;
using System.Collections.Generic;
using System.Linq;

namespace Dupont_Price_Lists.Services.Discounts
{
    public static class DiscountRuleSelector
    {
        public static DiscountRule? Select(
            IEnumerable<DiscountRule> rules,
            IEnumerable<string> lookupKeys,
            string sku,
            string customSku,
            string haystackLower)
        {
            var keySet = new HashSet<string>(lookupKeys.Where(k => !string.IsNullOrWhiteSpace(k)),
                StringComparer.OrdinalIgnoreCase);

            if (keySet.Count == 0) return null;

            // match any rule whose Keys intersects keySet
            var candidates = rules.Where(r => r.Keys.Any(k => keySet.Contains(k))).ToList();
            if (candidates.Count == 0) return null;

            // filter by TagContains / SkuStartsWith if provided
            var filtered = candidates.Where(r =>
            {
                bool tagOk = string.IsNullOrWhiteSpace(r.TagContains)
                    || haystackLower.Contains(r.TagContains.Trim().ToLowerInvariant())
                    || (customSku ?? "").ToLowerInvariant().Contains(r.TagContains.Trim().ToLowerInvariant());

                bool skuOk = string.IsNullOrWhiteSpace(r.SkuStartsWith)
                    || sku.StartsWith(r.SkuStartsWith.Trim(), StringComparison.OrdinalIgnoreCase);

                return tagOk && skuOk;
            }).ToList();

            return filtered.Count > 0 ? filtered[0] : candidates[0];
        }
    }
}
