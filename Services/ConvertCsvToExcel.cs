using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CsvHelper;

namespace Dupont_Price_Lists.Services
{
    public static class ConvertCsvToExcel
    {
        public static string ReadCsvFile_WriteExcelFile(string csxFilePath, string xlsxFileName)
        {
            var records = new List<string[]>();

            using (var reader = new StreamReader(csxFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                while (csv.Read())
                {
                    var row = new List<string>();
                    for (int i = 0; i < csv.Parser.Count; i++)
                    {
                        row.Add(csv.GetField(i));
                    }
                    records.Add(row.ToArray());
                }
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");

            worksheet.Cell(1, 1).InsertData(records);

            workbook.SaveAs(xlsxFileName);

            string fullPath = System.IO.Path.GetFullPath(xlsxFileName);

            return fullPath;
        }
    }
}
