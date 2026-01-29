using System;
using System.Windows.Forms;

namespace Dupont_Price_Lists.Services
{
    public static class FileManagerService
    {
        public static List<string>? OpenFile()
        {
            List<string> paths = new List<string>();
            using OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "CSV files (*.csv)|*.csv|XLSX files (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Select a CSV or XLSX file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                paths.Add(openFileDialog.FileName);

                if (openFileDialog.FilterIndex == 1)
                {
                    var filename = Path.GetFileName(openFileDialog.FileName);
                    string xlsxPath = "temp/" + filename.Replace(".csv", ".xlsx");

                    paths.Add(ConvertCsvToExcel.ReadCsvFile_WriteExcelFile(openFileDialog.FileName, xlsxPath));
                }
                return paths;
            }
            return null;
        }
    }
}
