using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Dupont_Price_Lists.Models;

namespace Dupont_Price_Lists.Services.Categories
{
    public interface ICategoryEngine
    {
        string? Resolve(ItemRecord record, IEnumerable<string> fieldsToScan);
    }

    public sealed class CategoryResolverHierarchical : ICategoryEngine
    {
        private List<CategoryToken> _tokens = new();
        private readonly string _separator;

        private sealed class CategoryToken
        {
            public string Token = "";
            public string FullPath = "";
            public int Depth;
        }

        private CategoryResolverHierarchical(string separator)
        {
            _separator = separator;
        }

        public static CategoryResolverHierarchical Load(string xlsxPath, string? sheetName, string separator)
        {
            if (string.IsNullOrEmpty(xlsxPath) || !System.IO.File.Exists(xlsxPath))
                throw new InvalidOperationException("Empty category sheet.");

            var engine = new CategoryResolverHierarchical(separator);

            using var wb = new XLWorkbook(xlsxPath);
            var ws = sheetName is null ? wb.Worksheets.First() : wb.Worksheet(sheetName);

            var firstRow = ws.FirstRowUsed() ?? throw new InvalidOperationException("Empty category sheet.");
            var headers = firstRow.CellsUsed().ToDictionary(c => c.GetString().Trim(), c => c.Address.ColumnNumber, StringComparer.OrdinalIgnoreCase);

            if (headers.ContainsKey("Path"))
                engine.LoadFromPathColumn(ws, headers["Path"]);
            else
                engine.LoadFromLevelColumns(ws);

            engine._tokens = engine._tokens
                .GroupBy(t => (t.Token, t.FullPath))
                .Select(g => g.First())
                .ToList();

            return engine;
        }

        public string? Resolve(ItemRecord record, IEnumerable<string> fieldsToScan)
        {
            var hay = string.Join(" ", fieldsToScan.Select(f => record.GetField(f) ?? ""))
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

            return best?.FullPath;
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
            var stack = new List<string>();

            for (int r = ws.FirstRowUsed().RowBelow().RowNumber(); r <= lastRow; r++)
            {
                var raw = ws.Cell(r, 1).GetString().Trim();
                if (string.IsNullOrEmpty(raw)) continue;

                var level = raw.Split(new[] { "--" }, StringSplitOptions.None).Length;
                var name = raw.Replace("-", "").Replace(">", "").Trim();

                while (stack.Count >= level) stack.RemoveAt(stack.Count - 1);
                stack.Add(name);

                var fullPath = string.Join(_separator, stack);

                _tokens.Add(new CategoryToken
                {
                    Token = name.ToLowerInvariant(),
                    FullPath = fullPath,
                    Depth = stack.Count
                });
            }
        }
    }
}
