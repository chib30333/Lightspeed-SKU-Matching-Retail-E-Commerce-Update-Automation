using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Dupont_Price_Lists.Models;

namespace Dupont_Price_Lists.Services
{
    public sealed class CategoryResolverHierarchical
    {
        private List<CategoryToken> _tokens = new();
        private readonly string _separator;

        private sealed class CategoryToken
        {
            public string Token = "";     // e.g., "glass shelves"
            public string FullPath = "";  // e.g., "Bathroom > Accessories > Glass Shelves"
            public int Depth;             // e.g., 3
        }

        public CategoryResolverHierarchical(
            string xlsxPath,
            string? sheetName,
            string separator,
            Func<string, string> normalizer // present for symmetry; not used in naive token build
        )
        {
            _separator = separator;

            using var wb = new XLWorkbook(xlsxPath);
            var ws = sheetName is null ? wb.Worksheets.First() : wb.Worksheet(sheetName);

            var firstRow = ws.FirstRowUsed() ?? throw new InvalidOperationException("Empty category sheet.");
            var headers = firstRow.Cells().ToDictionary(c => c.GetString().Trim(), c => c.Address.ColumnNumber);

            if (headers.Keys.Any(h => string.Equals(h, "Path", StringComparison.OrdinalIgnoreCase)))
                LoadFromPathColumn(ws, headers["Path"]);
            else
                LoadFromLevelColumns(ws);

            // Deduplicate
            _tokens = _tokens.GroupBy(t => (t.Token, t.FullPath)).Select(g => g.First()).ToList();
        }

        public string? Resolve(ItemRecord vendor, params string[] fieldsToScan)
        {
            var hay = string.Join(" ", fieldsToScan.Select(f => vendor.GetField(f) ?? ""))
                             .Trim()
                             .ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(hay)) return null;

            CategoryToken? best = null;

            foreach (var t in _tokens)
            {
                if (hay.Contains(t.Token))
                {
                    if (best == null ||
                        t.Depth > best.Depth ||
                        (t.Depth == best.Depth && t.Token.Length > best.Token.Length))
                        best = t;
                }
            }

            return best?.Token;
        }

        private void LoadFromPathColumn(IXLWorksheet ws, int pathCol)
        {
            int lastRow = ws.LastRowUsed().RowNumber();
            for (int r = ws.FirstRowUsed().RowBelow().RowNumber(); r <= lastRow; r++)
            {
                var rawPath = ws.Cell(r, pathCol).GetString().Trim();
                if (string.IsNullOrWhiteSpace(rawPath)) continue;

                var parts = rawPath
                    .Split(new[] { '>', '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .ToList();

                if (parts.Count == 0) continue;

                var fullPath = string.Join(_separator, parts);

                for (int i = 0; i < parts.Count; i++)
                {
                    _tokens.Add(new CategoryToken
                    {
                        Token = parts[i].ToLowerInvariant(),
                        FullPath = fullPath,
                        Depth = parts.Count
                    });
                }
            }
        }

        private void LoadFromLevelColumns(IXLWorksheet ws)
        {
            int lastRow = ws.LastRowUsed().RowNumber();
            List<string> categoryField = new List<string>();

            for (int r = ws.FirstRowUsed().RowBelow().RowNumber(); r <= lastRow; r++)
            {
                int deepLevel = 1;
                var val = ws.Cell(r, 1).GetString().Trim();

                if (!string.IsNullOrEmpty(val))
                {
                    var valArr = val.Split("--");
                    deepLevel = valArr.Count();

                    if (deepLevel > categoryField.Count)
                    {
                        categoryField.Add(valArr.Last());
                    }
                    else
                    {
                        categoryField[deepLevel - 1] = valArr.Last();
                        categoryField = categoryField.Slice(0, deepLevel);
                    }
                }

                var fullPath = string.Join("", categoryField);

                _tokens.Add(new CategoryToken
                {
                    Token = categoryField[categoryField.Count - 1].ToLowerInvariant().Replace("> ", ""),
                    FullPath = fullPath,
                    Depth = deepLevel
                });
            }
        }
    }
}
