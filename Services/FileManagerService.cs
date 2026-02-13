using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Dupont_Price_Lists.Services
{
    public static class FileManagerService
    {
        public static List<string>? OpenFile()
        {
            List<string> paths = new();

            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|XLSX files (*.xlsx)|*.xlsx",
                Title = "Select a CSV or XLSX file"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return null;

            paths.Add(openFileDialog.FileName);

            if (Path.GetExtension(openFileDialog.FileName).ToLowerInvariant() == ".csv")
            {
                Directory.CreateDirectory("temp");
                var filename = Path.GetFileName(openFileDialog.FileName);
                string xlsxPath = Path.Combine("temp", filename.Replace(".csv", ".xlsx"));
                paths.Add(ConvertCsvToExcel.ReadCsvFile_WriteExcelFile(openFileDialog.FileName, xlsxPath));
            }

            return paths;
        }
    }
}
