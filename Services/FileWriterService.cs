using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Dupont_Price_Lists.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Services
{
    public static class FileWriterService
    {
        public static class ExcelWriterService
        {
            public static void SaveUpdatedFile(List<ItemRecord> matchedItems, List<string> updateHeaders, Dictionary<string, string> pricingMap, string outputFilePath)
            {
                if (string.IsNullOrEmpty(outputFilePath))
                    throw new ArgumentException("Output file path cannot be empty.", nameof(outputFilePath));

                if (matchedItems == null || matchedItems.Count == 0)
                    throw new ArgumentException("No matched items to save.", nameof(matchedItems));

                var headers = updateHeaders.ToList();
                if (!headers.Contains(pricingMap["RetailField"])) headers.Add(pricingMap["RetailField"]);
                if (!headers.Contains(pricingMap["EcomField"])) headers.Add(pricingMap["EcomField"]);

                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("New");

                for (int i = 0; i < headers.Count; i++)
                    ws.Cell(1, i + 1).Value = headers[i];

                int row = 2;
                foreach (var item in matchedItems)
                {
                    for (int c = 0; c < headers.Count; c++)
                        ws.Cell(row, c + 1).Value = item.GetField(headers[c]) ?? "";
                    row++;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(outputFilePath);

            }

            public static void SaveFound(string filePath, List<FoundMatch> foundMatches, List<string> vendorHeaders, string lightspeedIdField = "System ID")
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Found");

                var headers = vendorHeaders.ToList();
                if (!headers.Contains(lightspeedIdField)) headers.Add(lightspeedIdField);

                for (int i = 0; i < headers.Count; i++)
                    ws.Cell(1, i + 1).Value = headers[i];

                int row = 2;
                foreach (var fm in foundMatches)
                {
                    for (int c = 0; c < vendorHeaders.Count; c++)
                        ws.Cell(row, c + 1).Value = fm.Vendor.GetField(vendorHeaders[c]) ?? "";

                    ws.Cell(row, vendorHeaders.Count + 1).Value = fm.LightspeedPrimary?.GetField(lightspeedIdField) ?? "";
                    row++;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }

            public static void SaveNew(string filePath, List<ItemRecord> newItems, List<string> vendorHeaders)
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("New");

                for (int i = 0; i < vendorHeaders.Count; i++)
                    ws.Cell(1, i + 1).Value = vendorHeaders[i];

                int row = 2;
                foreach (var item in newItems)
                {
                    for (int c = 0; c < vendorHeaders.Count; c++)
                        ws.Cell(row, c + 1).Value = item.GetField(vendorHeaders[c]) ?? "";
                    row++;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }

            public static void SaveReport(string filePath, MatchResult result)
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Report");
                ws.Cell(1, 1).Value = "Total Vendor Rows";
                ws.Cell(1, 2).Value = result.TotalVendorRows;
                ws.Cell(2, 1).Value = "Total Lightspeed Rows";
                ws.Cell(2, 2).Value = result.TotalLightspeedRows;
                ws.Cell(3, 1).Value = "Found";
                ws.Cell(3, 2).Value = result.Found.Count;
                ws.Cell(4, 1).Value = "New";
                ws.Cell(4, 2).Value = result.NewItems.Count;
                ws.Cell(6, 1).Value = "Vendor Duplicates Count";
                ws.Cell(6, 2).Value = result.VendorDuplicates.Count;
                ws.Cell(7, 1).Value = "Lightspeed Duplicates Count";
                ws.Cell(7, 2).Value = result.LightspeedDuplicates.Count;

                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }

            public static void SaveLightspeedDuplicatesToExcel(List<ItemRecord> lightspeedDuplicates, string outputFilePath, FieldMapping mapping)
            {
                var dt = new DataTable();
                dt.Columns.Add("Sku");
                dt.Columns.Add("Description");
                // Add more columns as needed

                foreach (var item in lightspeedDuplicates)
                {
                    var row = dt.NewRow();
                    row["Sku"] = item.GetField(mapping.VendorSkuField);
                    row["Description"] = item.GetField(mapping.VendorDescriptionField);
                    // Populate other fields
                    dt.Rows.Add(row);
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Lightspeed Duplicates");
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    package.SaveAs(new FileInfo(outputFilePath));
                }
            }
        }
    }
}
