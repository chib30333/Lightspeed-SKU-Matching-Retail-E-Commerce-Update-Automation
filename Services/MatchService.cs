using Dupont_Price_Lists.Forms;
using Dupont_Price_Lists.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dupont_Price_Lists.Services.OnlineBuilder;

namespace Dupont_Price_Lists.Services
{
    public class MatchRetailOptions
    {
        public bool IgnoreVendorDuplicates { get; set; } = true;

        public bool ArchiveLightspeedDuplicates { get; set; } = true;

        public Func<string, string> KeyNormalizer { get; set; } = DefaultNormalizer;

        public static string DefaultNormalizer(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;
            return string.Join(" ", str.Trim().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                         .ToUpperInvariant();
        }
    }
    public static class MatchRetailService
    {
        public static MatchResult MatchItems(
            IEnumerable<ItemRecord> vendorData,
            IEnumerable<ItemRecord> lightspeedData,
            FieldMapping mapping,
            MatchRetailOptions? options = null)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));
            options ??= new MatchRetailOptions();

            var result = new MatchResult();

            var lsIndex = new Dictionary<string, List<ItemRecord>>(StringComparer.OrdinalIgnoreCase);
            int lsCount = 0;

            foreach (var ls in lightspeedData)
            {
                lsCount++;
                var raw = ls.GetField(mapping.LightspeedSkuField);
                var key = options.KeyNormalizer(raw);
                if (string.IsNullOrEmpty(key)) continue;

                if (!lsIndex.TryGetValue(key, out var list))
                {
                    list = new List<ItemRecord>(capacity: 1);
                    lsIndex[key] = list;
                }
                list.Add(ls);
            }

            result.TotalLightspeedRows = lsCount;

            if (options.ArchiveLightspeedDuplicates)
            {
                foreach (var kv in lsIndex)
                {
                    if (kv.Value.Count > 1)
                        result.LightspeedDuplicates[kv.Key] = new List<ItemRecord>(kv.Value);
                }
            }

            var seenVendor = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int vendorCount = 0;

            foreach (var vendor in vendorData)
            {
                vendorCount++;
                var rawVendor = vendor.GetField(mapping.VendorSkuField);
                var key = options.KeyNormalizer(rawVendor);

                if (string.IsNullOrEmpty(key)) continue;

                if (options.IgnoreVendorDuplicates)
                {
                    if (seenVendor.Contains(key))
                    {
                        if (!result.VendorDuplicates.TryGetValue(key, out var list))
                        {
                            list = new List<ItemRecord>();
                            result.VendorDuplicates[key] = list;
                        }
                        list.Add(vendor);
                        continue;
                    }
                    seenVendor.Add(key);
                }

                if (lsIndex.TryGetValue(key, out var matchingLsList))
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
                        lightspeedItem = matchingLsList.First(),
                        vendorItem = vendor,
                        specify = "Found"
                    });
                }
                else
                {
                    result.NewItems.Add(vendor);
                    result.Retail.Add(new RetailMatch
                    {
                        lightspeedItem = null,
                        vendorItem = vendor,
                        specify = "New"
                    });
                }
            }

            result.TotalVendorRows = vendorCount;
            return result;
        }
    }
}
