using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Dupont_Price_Lists.Services.Matching;

namespace Dupont_Price_Lists.Services.Discounts
{
    public sealed class DiscountRuleProvider
    {
        private readonly Func<string?, string?> _norm;

        public DiscountRuleProvider(Func<string?, string?> normalizer)
        {
            _norm = normalizer;
        }

        public List<DiscountRule> Load(string xlsxPath, string? sheetName = null)
        {
            if (string.IsNullOrWhiteSpace(xlsxPath))
                throw new ArgumentException("Discount sheet missing path.");

            using var wb = new XLWorkbook(xlsxPath);
            var ws = sheetName is null ? wb.Worksheets.First() : wb.Worksheet(sheetName);

            var headerRow = ws.FirstRowUsed() ?? throw new InvalidOperationException("Empty discount sheet.");
            var headers = headerRow.CellsUsed()
                .Select(c => c.GetString().Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToDictionary(h => h, h => headerRow.CellsUsed().First(c => c.GetString().Trim().Equals(h, StringComparison.OrdinalIgnoreCase)).Address.ColumnNumber,
                    StringComparer.OrdinalIgnoreCase);

            string GetCell(int r, string col)
                => headers.TryGetValue(col, out var c) ? ws.Cell(r, c).GetString().Trim() : "";

            int lastRow = ws.LastRowUsed().RowNumber();

            var rules = new List<DiscountRule>();

            for (int r = headerRow.RowBelow().RowNumber(); r <= lastRow; r++)
            {
                var brand = GetCell(r, "Brand");
                var brandKey = _norm(brand) ?? "";

                if (string.IsNullOrWhiteSpace(brandKey))
                    continue;

                var rule = new DiscountRule
                {
                    BrandKey = brandKey,
                    TagContains = headers.ContainsKey("TagContains") ? GetCell(r, "TagContains") : null,
                    SkuStartsWith = headers.ContainsKey("SkuStartsWith") ? GetCell(r, "SkuStartsWith") : null,

                    DefaultCostRule = TryGetAny(GetCell, r, "Default Cost", "DefaultCost"),
                    VendorCostRule = TryGetAny(GetCell, r, "Vendor Cost", "VendorCost"),
                    DefaultPriceRule = TryGetAny(GetCell, r, "Default Price", "DefaultPrice"),
                    RetailPriceRule = TryGetAny(GetCell, r, "Retail Price", "RetailPrice"),
                    ContractorPriceRule = TryGetAny(GetCell, r, "Contractor Price", "ContractorPrice"),
                    DesignerPriceRule = TryGetAny(GetCell, r, "Designer Price", "DesignerPrice"),
                    OnlinePriceRule = TryGetAny(GetCell, r, "Online Price", "OnlinePrice"),
                    VipPriceRule = TryGetAny(GetCell, r, "V.I.P Price", "VIP Price", "VipPrice", "V.I.P", "VIP"),
                };

                rules.Add(rule);
            }

            return rules;
        }

        private static string? TryGetAny(Func<int, string, string> getCell, int r, params string[] cols)
        {
            foreach (var c in cols)
            {
                var v = getCell(r, c);
                if (!string.IsNullOrWhiteSpace(v)) return v;
            }
            return null;
        }
    }
}
