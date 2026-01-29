using ClosedXML.Excel;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Dupont_Price_Lists.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services
{

    // ------------------ DISCOUNT ------------------
    public sealed class DiscountResolver
    {
        private readonly List<Row> _rows = new();
        private readonly Func<string, string> _norm;

        public sealed class Row
        {
            public string BrandKey = "";
            public Dictionary<string, string> Rule = new Dictionary<string, string>();            // e.g., "50/10"
            public string? TagContains;         // optional
            public string? SkuStartsWith;       // optional
        }

        public DiscountResolver(
            string xlsxPath,
            string? sheetName,
            string brandColumn,
            string unit,
            string? tagContainsColumn,
            string? skuStartsWithColumn,
            Func<string, string> normalizer)
        {
            _norm = normalizer;

            using var wb = new XLWorkbook(xlsxPath);
            var ws = sheetName is null ? wb.Worksheets.First() : wb.Worksheet(sheetName);

            var header = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed().RowNumber();
            var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var cell in header.CellsUsed())
            {
                var name = cell.GetString()?.Trim();
                if (string.IsNullOrEmpty(name)) continue;           // skip blanks
                if (!headers.ContainsKey(name))                     // keep the FIRST occurrence
                    headers[name] = cell.Address.ColumnNumber;
                // else: ignore duplicates (or log them)
            }

            int bCol = headers[brandColumn];
            int? tCol = tagContainsColumn is not null && headers.TryGetValue(tagContainsColumn, out var tc) ? tc : null;
            int? sCol = skuStartsWithColumn is not null && headers.TryGetValue(skuStartsWithColumn, out var sc) ? sc : null;

            for (int r = header.RowBelow().RowNumber(); r <= lastRow; r++)
            {
                int rCol;
                var brand = ws.Cell(r, bCol).GetString().Trim();
                Dictionary<string, string> rules = new Dictionary<string, string>();

                foreach (var headerCell in headers)
                {
                    if (headerCell.Key.Contains("Off") || headerCell.Key.Contains("Default Price") || headerCell.Key.Contains("MSRP"))
                    {
                        rCol = headerCell.Value;
                        rules.Add(headerCell.Key, ws.Cell(r, rCol).GetString().Trim());
                    }
                }

                _rows.Add(new Row
                {
                    BrandKey = _norm(brand) ?? "",
                    Rule = rules,
                    TagContains = tCol is null ? null : ws.Cell(r, tCol.Value).GetString().Trim(),
                    SkuStartsWith = sCol is null ? null : ws.Cell(r, sCol.Value).GetString().Trim(),
                });
            }
        }

        private static int FindColumn(IXLRow headerRow, string wantedName)
        {
            foreach (var cell in headerRow.CellsUsed())
            {
                var name = cell.GetString()?.Trim();
                if (!string.IsNullOrEmpty(name) &&
                    string.Equals(name, wantedName, StringComparison.OrdinalIgnoreCase))
                {
                    return cell.Address.ColumnNumber;
                }
            }
            throw new InvalidOperationException($"Master Discount List missing column '{wantedName}'.");
        }

        public Dictionary<string, string>? ResolveRule(ItemRecord vendor, string brandField, string skuField, string customSkuField, params string[] maybeTextFields)
        {
            var brandKey = _norm(vendor.GetField(brandField)) ?? "";
            var sku = vendor.GetField(skuField) ?? "";
            var customSku = vendor.GetField(customSkuField) ?? "";
            var hay = string.Join(" ", maybeTextFields.Select(vendor.GetField)).ToLowerInvariant();

            var matches = _rows.Where(r => r.BrandKey == brandKey);

            var withFilters = matches.Where(r =>
                (string.IsNullOrEmpty(r.TagContains) || hay.Contains(r.TagContains!.ToLowerInvariant()) || customSku.ToLowerInvariant().Contains(r.TagContains!.ToLowerInvariant())) &&
                (string.IsNullOrEmpty(r.SkuStartsWith) || sku.StartsWith(r.SkuStartsWith!, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            if (withFilters.Count > 0) return withFilters.First().Rule;

            var brandOnly = matches.FirstOrDefault();
            return brandOnly?.Rule;
        }
    }
}
