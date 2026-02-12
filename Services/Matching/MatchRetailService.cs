using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Matching;

namespace Dupont_Price_Lists.Services.Matching
{
    public sealed class MatchRetailOptions
    {
        public bool IgnoreVendorDuplicates { get; set; } = true;
        public bool TrackLightspeedDuplicates { get; set; } = true;
        public Func<string?, string?> KeyNormalizer { get; set; } = DefaultNormalizer;

        public static string? DefaultNormalizer(string? str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;
            var cleaned = string.Join(" ", str.Trim()
                .Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            return cleaned.ToUpperInvariant();
        }
    }

    public static class MatchRetailService
    {
        public static Task<MatchResult> MatchItemsAsync(
            IEnumerable<ItemRecord> vendorData,
            IEnumerable<ItemRecord> lightspeedData,
            MappingProfile profile,
            MatchRetailOptions? options = null,
            CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                options ??= new MatchRetailOptions();
                var result = new MatchResult();

                // Index Lightspeed by normalized SKU
                var lsIndex = new Dictionary<string, List<ItemRecord>>(StringComparer.OrdinalIgnoreCase);
                int lsCount = 0;

                foreach (var ls in lightspeedData)
                {
                    ct.ThrowIfCancellationRequested();
                    lsCount++;

                    var key = options.KeyNormalizer(ls.GetField(profile.LightspeedSkuField));
                    if (string.IsNullOrEmpty(key)) continue;

                    if (!lsIndex.TryGetValue(key, out var list))
                        lsIndex[key] = list = new List<ItemRecord>();
                    list.Add(ls);
                }
                result.TotalLightspeedRows = lsCount;

                if (options.TrackLightspeedDuplicates)
                {
                    foreach (var kv in lsIndex)
                        if (kv.Value.Count > 1)
                            result.LightspeedDuplicates[kv.Key] = new List<ItemRecord>(kv.Value);
                }

                // Walk Vendor rows
                var seenVendor = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                int vendorCount = 0;

                foreach (var vendor in vendorData)
                {
                    ct.ThrowIfCancellationRequested();
                    vendorCount++;

                    var vKey = options.KeyNormalizer(vendor.GetField(profile.VendorSkuField));
                    if (string.IsNullOrEmpty(vKey)) continue;

                    if (options.IgnoreVendorDuplicates && !seenVendor.Add(vKey))
                    {
                        if (!result.VendorDuplicates.TryGetValue(vKey, out var dups))
                            result.VendorDuplicates[vKey] = dups = new List<ItemRecord>();
                        dups.Add(vendor);
                        continue;
                    }

                    if (lsIndex.TryGetValue(vKey, out var matchingLsList))
                    {
                        var fm = new FoundMatch
                        {
                            Vendor = vendor,
                            LightspeedPrimary = matchingLsList.First(),
                            LightspeedAll = new List<ItemRecord>(matchingLsList)
                        };
                        result.Found.Add(fm);
                        result.Retail.Add(new RetailMatch
                        {
                            LightspeedItem = matchingLsList.First(),
                            VendorItem = vendor,
                            RecordType = "Found"
                        });
                    }
                    else
                    {
                        result.NewItems.Add(vendor);
                        result.Retail.Add(new RetailMatch
                        {
                            LightspeedItem = null,
                            VendorItem = vendor,
                            RecordType = "New"
                        });
                    }
                }

                result.TotalVendorRows = vendorCount;
                return result;
            }, ct);
        }
    }
}
