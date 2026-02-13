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

        public List<DiscountRule> Load(
            string xlsxPath,
            string? sheetName = null,
            IEnumerable<string>? keyColumns = null)
        {
            keyColumns ??= new[] { "Brand", "vendor", "lyncar" }; // fallback order

            using var wb = new XLWorkbook(xlsxPath);
            var ws = sheetName is null ? wb.Worksheets.First() : wb.Worksheet(sheetName);

            var headerRow = ws.FirstRowUsed() ?? throw new InvalidOperationException("Empty discount sheet.");
            var headerCells = headerRow.CellsUsed().ToList();

            // header map: name -> column number
            var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var cell in headerCells)
            {
                var name = cell.GetString().Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;
                if (!headers.ContainsKey(name))
                    headers[name] = cell.Address.ColumnNumber;
            }

            string GetCell(int r, string colName)
            {
                return headers.TryGetValue(colName, out var c)
                    ? ws.Cell(r, c).GetString().Trim()
                    : "";
            }

            int lastRow = ws.LastRowUsed().RowNumber();
            var rules = new List<DiscountRule>();

            for (int r = headerRow.RowBelow().RowNumber(); r <= lastRow; r++)
            {
                // Build all possible keys for this row
                var keys = new List<string>();
                foreach (var col in keyColumns)
                {
                    var raw = GetCell(r, col);
                    var k = _norm(raw);
                    if (!string.IsNullOrWhiteSpace(k) && !keys.Contains(k, StringComparer.OrdinalIgnoreCase))
                        keys.Add(k);
                }

                // If none of Brand/vendor/lyncar exists, skip row
                if (keys.Count == 0)
                    continue;

                var rule = new DiscountRule
                {
                    Keys = keys,
                    TagContains = headers.ContainsKey("TagContains") ? GetCell(r, "TagContains") : null,
                    SkuStartsWith = headers.ContainsKey("SkuStartsWith") ? GetCell(r, "SkuStartsWith") : null,

                    DefaultCostRule = TryGetAny(GetCell, r, "Default Cost - % Off", "Default Cost", "DefaultCost"),
                    VendorCostRule = TryGetAny(GetCell, r, "Vendor Cost - % Off", "Vendor Cost", "VendorCost"),
                    DefaultPriceRule = TryGetAny(GetCell, r, "Default Price", "DefaultPrice"),
                    RetailPriceRule = TryGetAny(GetCell, r, "Retail/Contractor - % Off", "Retail Price", "RetailPrice"),
                    ContractorPriceRule = TryGetAny(GetCell, r, "Contractor - % Off", "Contractor Price", "ContractorPrice"),
                    DesignerPriceRule = TryGetAny(GetCell, r, "Designer - % Off", "Designer Price", "DesignerPrice"),
                    OnlinePriceRule = TryGetAny(GetCell, r, "Online - % Off", "Online Price", "OnlinePrice"),
                    VipPriceRule = TryGetAny(GetCell, r, "V.I.P. - % Off", "V.I.P Price", "VIP Price", "VipPrice")
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
