using System.Collections.Generic;
using Dupont_Price_Lists.Models;

namespace Dupont_Price_Lists.Models.Matching
{
    public sealed class FoundMatch
    {
        public required ItemRecord Vendor { get; set; }
        public required ItemRecord LightspeedPrimary { get; set; }
        public List<ItemRecord> LightspeedAll { get; set; } = new();
    }

    public sealed class RetailMatch
    {
        public ItemRecord? LightspeedItem { get; set; }
        public required ItemRecord VendorItem { get; set; }
        public string RecordType { get; set; } = ""; // Found/New
    }

    public sealed class MatchResult
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
