using Dupont_Price_Lists.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dupont_Price_Lists.Services.FileReaderService;

namespace Dupont_Price_Lists.Services
{
    public static class UnifiedReaderService
    {
        public static (List<string> headers, List<ItemRecord> rows) ReadAll(string filePath, int sheetIndex, string? brand, string? vendor)
        {
            var ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".xlsx" => ExcelReaderService.ReadAll(filePath, sheetIndex, brand, vendor),
                ".csv" => CsvReaderService.ReadAll(filePath, sheetIndex, brand, vendor),
                _ => throw new NotSupportedException("Only .xlsx and .csv are supported."),
            };
        }
    }
}
