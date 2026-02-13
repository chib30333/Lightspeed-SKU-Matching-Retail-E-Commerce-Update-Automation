using System.Collections.Generic;
using Dupont_Price_Lists.Models.Matching;
using Dupont_Price_Lists.Models.Outputs;

namespace Dupont_Price_Lists.Services.Pipeline
{
    public sealed class RetailBuildResult
    {
        public List<RetailRow> Rows { get; init; } = new();
        public int Found { get; init; }
        public int NewItems { get; init; }
        public int VendorDuplicateKeys { get; init; }
        public int LightspeedDuplicateKeys { get; init; }
        public MatchResult Match { get; init; } = new();
        public List<string> Warnings { get; init; } = new();
    }

    public sealed class OnlineBuildResult
    {
        public List<OnlineRow> Rows { get; init; } = new();
        public List<string> Warnings { get; init; } = new();
    }
}
