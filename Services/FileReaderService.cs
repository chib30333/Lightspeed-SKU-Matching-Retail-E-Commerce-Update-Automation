using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using Dupont_Price_Lists.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services
{
    public static class FileReaderService
    {
        public static class ExcelReaderService

        {
            public static (List<string> headers, List<ItemRecord> rows) ReadAll(string filePath, int sheetIndex, string? brand, string? vendor)
            {
                var headers = new List<string>();
                var rows = new List<ItemRecord>();

                using var wb = new XLWorkbook(filePath);

                var ws = wb.Worksheets.Worksheet(sheetIndex);

                var firstRow = ws.FirstRowUsed();
                if (firstRow == null) return (headers, rows);

                var lastCell = firstRow.LastCellUsed();
                int lastCol = lastCell.Address.ColumnNumber;

                for (int c = 1; c <= lastCol; c++)
                {
                    var raw = firstRow.Cell(c).GetString().Trim().Replace(".", "");
                    headers.Add(string.IsNullOrEmpty(raw) ? $"Column{c}" : raw);
                }

                var dataRows = ws.RowsUsed().Skip(1);
                foreach (var r in dataRows)
                {
                    var record = new ItemRecord();
                    for (int c = 1; c <= headers.Count; c++)
                    {
                        var v = r.Cell(c).GetString();
                        record.SetField(headers[c - 1], v);
                    }
                    if (brand != "" || vendor != "")
                    {
                        if(brand != "" && vendor == "")
                        {
                            if (record.GetField("brand") == brand)
                            {
                                rows.Add(record);
                            }
                        } else if(brand == "" && vendor != "")
                        {
                            if (record.GetField("vendor") == vendor)
                            {
                                rows.Add(record);
                            }

                        } else if(vendor != "" && brand != "")
                        {
                            if (record.GetField("vendor") == vendor && record.GetField("brand") == brand)
                            {
                                rows.Add(record);
                            }
                        }
                    } else
                    {
                        rows.Add(record);
                    }
                }

                return (headers, rows);
            }

            public static async Task<List<ItemRecord>> ReadExcelAsync(string filePath, string? sheetName = null)
            {
                return await Task.Run(() =>
                {
                    var records = new List<ItemRecord>();

                    using (var workbook = new XLWorkbook(filePath))
                    {
                        var worksheet = string.IsNullOrEmpty(sheetName) ?
                            workbook.Worksheet(1) :
                            workbook.Worksheet(sheetName);

                        var headerRow = worksheet.FirstRowUsed();
                        var headers = new List<string>();

                        foreach (var cell in headerRow.Cells())
                        {
                            string cleanHeader = cell.GetString().Trim();
                            if (!string.IsNullOrEmpty(cleanHeader))
                                headers.Add(cleanHeader);
                        }

                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var record = new ItemRecord();
                            for (int i = 0; i < headers.Count; i++)
                            {
                                string header = headers[i];
                                string value = row.Cell(i + 1).GetString().Trim();
                                record.SetField(header, value);
                            }
                            records.Add(record);
                        }
                    }

                    return records;
                });
            }

            public static void ReadExcelToShowText(string filePath, Action<bool, string> onRowRead)
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);

                int limit = 0;
                bool isFirstRow = true;

                do
                {
                    while (reader.Read())
                    {

                        if (limit > 500) return;
                        string rowText = "";

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string cellValue = reader.GetValue(i)?.ToString().Trim() ?? string.Empty;

                            rowText += cellValue + "\t";
                        }
                        onRowRead?.Invoke(isFirstRow, rowText.Trim());

                        limit++;
                    }


                } while (reader.NextResult());
            }
        }

        public static class CsvReaderService
        {
            public static (List<string> headers, List<ItemRecord> rows) ReadAll(string filePath, int? sheetIndex, string? brand, string? vendor)
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
                var headers = new List<string>(csv.HeaderRecord);

                while (csv.Read())
                {
                    var rec = new ItemRecord();
                    foreach (var h in headers)
                    {
                        var val = csv.GetField(h);
                        rec.SetField(h, val);
                    }
                    rows.Add(rec);
                }

                return (headers, rows);
            }

            public static IEnumerable<string[]> ReadCsv(string path)
            {
                using var reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (line == null)
                    {
                        continue;
                    }
                    else
                    {
                        yield return line.Split(',');
                    }
                }
            }
        }

    }
}
