using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Services.Builders;
using Dupont_Price_Lists.Services.Categories;
using Dupont_Price_Lists.Services.Discounts;
using Dupont_Price_Lists.Services.Matching;
using Dupont_Price_Lists.Services.Profiles;
using Dupont_Price_Lists.Services.Writing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services.Pipeline
{
    public sealed class PipelineOrchestrator
    {
        private readonly RetailBuilder _retailBuilder = new();
        private readonly OnlineBuilder _onlineBuilder = new();
        private readonly ExcelWriter2 _writer = new();

        public async Task<RetailBuildResult> BuildRetailAsync(
            string vendorPath,
            string lightspeedPath,
            string categoryPath,
            string discountPath,
            MappingProfile profile,
            CancellationToken ct = default)
        {
            var (vendorHeaders, vendorRows) = UnifiedReaderService.ReadAll(vendorPath, 1);
            var (lsHeaders, lightspeedRows) = UnifiedReaderService.ReadAll(lightspeedPath, 1);

            profile.VendorDescriptionField ??= VendorFieldAutoMapper.GuessField(vendorHeaders, "Description", "Item Description", "Desc");
            profile.VendorFinishField ??= VendorFieldAutoMapper.GuessField(vendorHeaders, "Finish", "Color", "Finish Name");
            profile.VendorPriceField ??= VendorFieldAutoMapper.GuessField(vendorHeaders, "MSRP", "List Price", "New List Price", "Price");
            profile.VendorUpcField ??= VendorFieldAutoMapper.GuessField(vendorHeaders, "UPC", "Upc", "Barcode");

            var match = await MatchRetailService.MatchItemsAsync(vendorRows, lightspeedRows, profile, ct: ct);

            var categoryEngine = CategoryResolverHierarchical.Load(categoryPath, sheetName: null, separator: profile.CategorySeparator);

            var discountProvider = new DiscountRuleProvider(MatchRetailOptions.DefaultNormalizer);
            var rules = discountProvider.Load(discountPath);

            var retailRows = _retailBuilder.Build(match, profile, categoryEngine, rules);

            var warnings = new List<string>();
            if (retailRows.Count > 10_000)
                warnings.Add($"Retail rows ({retailRows.Count}) exceed 10,000. Implement split-export or reduce scope.");

            return new RetailBuildResult
            {
                Rows = retailRows,
                Found = match.Found.Count,
                NewItems = match.NewItems.Count,
                VendorDuplicateKeys = match.VendorDuplicates.Count,
                LightspeedDuplicateKeys = match.LightspeedDuplicates.Count,
                Warnings = warnings
            };
        }

        public async Task<OnlineBuildResult> BuildOnlineAsync(
            string onlinePath,
            string vendorPath,
            string lightspeedPath,
            string discountPath,
            MappingProfile profile,
            CancellationToken ct = default)
        {
            var (_, vendorRows) = UnifiedReaderService.ReadAll(vendorPath, 1);
            var (_, lightspeedRows) = UnifiedReaderService.ReadAll(lightspeedPath, 1);
            var (_, onlineRows) = UnifiedReaderService.ReadAll(onlinePath, 1);

            var match = await MatchRetailService.MatchItemsAsync(vendorRows, lightspeedRows, profile, ct: ct);

            var discountProvider = new DiscountRuleProvider(MatchRetailOptions.DefaultNormalizer);
            var rules = discountProvider.Load(discountPath);

            var outRows = _onlineBuilder.Build(match, onlineRows, profile, rules);

            var warnings = new List<string>();
            // optional validations
            return new OnlineBuildResult { Rows = outRows, Warnings = warnings };
        }

        public Task WriteRetailAsync(string outputPath, List<Models.Outputs.RetailRow> rows, CancellationToken ct = default)
            => _writer.WriteRetailAsync(outputPath, rows, ct);

        public Task WriteOnlineAsync(string outputPath, List<Models.Outputs.OnlineRow> rows, CancellationToken ct = default)
            => _writer.WriteOnlineAsync(outputPath, rows, ct);
    }
}
