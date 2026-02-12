using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using Dupont_Price_Lists.Models;

namespace Dupont_Price_Lists.Services.Readers
{
    public static class FileReaderService
    {
        public static class ExcelReaderService
        {
            public static (List<string> headers, List<ItemRecord> rows) ReadAll(string filePath, int sheetIndex)
            {
                var headers = new List<string>();
                var rows = new List<ItemRecord>();

                using var wb = new XLWorkbook(filePath);
                var ws = wb.Worksheet(sheetIndex);

                var firstRow = ws.FirstRowUsed();
                if (firstRow == null) return (headers, rows);

                var lastCol = firstRow.LastCellUsed().Address.ColumnNumber;

                for (int c = 1; c <= lastCol; c++)
                {
                    var raw = firstRow.Cell(c).GetString().Trim();
                    headers.Add(string.IsNullOrEmpty(raw) ? $"Column{c}" : raw);
                }

                foreach (var r in ws.RowsUsed().Skip(1))
                {
                    var record = new ItemRecord();
                    for (int c = 1; c <= headers.Count; c++)
                    {
                        record.SetField(headers[c - 1], r.Cell(c).GetString());
                    }
                    rows.Add(record);
                }

                return (headers, rows);
            }
        }

        public static class CsvReaderService
        {
            public static (List<string> headers, List<ItemRecord> rows) ReadAll(string filePath)
            {
                var rows = new List<ItemRecord>();
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true,
                    TrimOptions = TrimOptions.Trim,
                });

                csv.Read();
                csv.ReadHeader();
                var headers = csv.HeaderRecord?.ToList() ?? new List<string>();

                while (csv.Read())
                {
                    var rec = new ItemRecord();
                    foreach (var h in headers)
                        rec.SetField(h, csv.GetField(h) ?? "");
                    rows.Add(rec);
                }

                return (headers, rows);
            }
        }
    }
}
