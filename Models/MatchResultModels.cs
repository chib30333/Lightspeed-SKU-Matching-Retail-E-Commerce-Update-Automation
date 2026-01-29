using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Models
{
    public static class MatchResultModels
    {
    }

    public class FoundMatch
    {
        public required ItemRecord Vendor { get; set; }
        public required ItemRecord LightspeedPrimary { get; set; }
        public List<ItemRecord> LightspeedAll { get; set; } = new();
    }

    public class RetailMatch
    {
        public required ItemRecord lightspeedItem { get; set; }
        public required ItemRecord vendorItem { get; set; }
        public string? specify {  get; set; }
    }

    public class MatchResult
    {
        public List<FoundMatch> Found { get; set; } = new();
        public List<ItemRecord> NewItems { get; } = new();
        public List<RetailMatch> Retail { get; set; } = new();
        public Dictionary<string, List<ItemRecord>> VendorDuplicates { get; } = new();
        public Dictionary<string, List<ItemRecord>> LightspeedDuplicates { get; } = new();
        public int TotalVendorRows { get; set; }
        public int TotalLightspeedRows { get; set; }
    }
}
