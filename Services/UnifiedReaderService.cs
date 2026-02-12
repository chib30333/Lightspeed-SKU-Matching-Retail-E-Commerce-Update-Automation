using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Services.Readers;
using System;
using System.Collections.Generic;

namespace Dupont_Price_Lists.Services
{
    public static class UnifiedReaderService
    {
        public static (List<string> headers, List<ItemRecord> rows) ReadAll(string filePath, int sheetIndex)
        {
            var ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".xlsx" => FileReaderService.ExcelReaderService.ReadAll(filePath, sheetIndex),
                ".csv" => FileReaderService.CsvReaderService.ReadAll(filePath),
                _ => throw new NotSupportedException("Only .xlsx and .csv are supported."),
            };
        }
    }
}
