using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Dupont_Price_Lists.Models.Outputs;

namespace Dupont_Price_Lists.Services.Writing
{
    public sealed class ExcelWriter2
    {
        public Task WriteRetailAsync(string path, List<RetailRow> rows, CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Retail");

                var headers = new[]
                {
                    "System ID","Manufact. SKU","Custom SKU","UPC","Brand","Vendor","Description","Finish","Category",
                    "MSRP","Default Cost","Vendor Cost","Default Price","Retail Price","Contractor Price","Designer Price","Online Price","V.I.P Price",
                    "Archive","Record Type"
                };

                for (int c = 0; c < headers.Length; c++)
                    ws.Cell(1, c + 1).Value = headers[c];

                int r = 2;
                foreach (var x in rows)
                {
                    ct.ThrowIfCancellationRequested();

                    ws.Cell(r, 1).Value = x.SystemId ?? "";
                    ws.Cell(r, 2).Value = x.ManufactSku ?? "";
                    ws.Cell(r, 3).Value = x.CustomSku ?? "";
                    ws.Cell(r, 4).Value = x.Upc ?? "";
                    ws.Cell(r, 5).Value = x.Brand ?? "";
                    ws.Cell(r, 6).Value = x.Vendor ?? "";
                    ws.Cell(r, 7).Value = x.Description ?? "";
                    ws.Cell(r, 8).Value = x.Finish ?? "";
                    ws.Cell(r, 9).Value = x.Category ?? "";

                    ws.Cell(r, 10).Value = x.Msrp.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 11).Value = x.DefaultCost.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 12).Value = x.VendorCost.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 13).Value = x.DefaultPrice.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 14).Value = x.RetailPrice.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 15).Value = x.ContractorPrice.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 16).Value = x.DesignerPrice.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 17).Value = x.OnlinePrice.ToString("0.##", CultureInfo.InvariantCulture);
                    ws.Cell(r, 18).Value = x.VipPrice.ToString("0.##", CultureInfo.InvariantCulture);

                    ws.Cell(r, 19).Value = x.Archive ? "Y" : "N";
                    ws.Cell(r, 20).Value = x.RecordType ?? "";

                    r++;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(path);
            }, ct);
        }

        public Task WriteOnlineAsync(string path, List<OnlineRow> rows, CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Online");

                var headers = new[]
                {
                    "System ID","Manufact. SKU","Variant ID","Brand","Description","Category","Ecom","Online Price",
                    "Shipping Weight","Shipping Box Dimensions A","Shipping Box Dimensions B","Shipping Box Dimensions C"
                };

                for (int c = 0; c < headers.Length; c++)
                    ws.Cell(1, c + 1).Value = headers[c];

                int r = 2;
                foreach (var x in rows)
                {
                    ct.ThrowIfCancellationRequested();

                    ws.Cell(r, 1).Value = x.SystemId ?? "";
                    ws.Cell(r, 2).Value = x.ManufactSku ?? "";
                    ws.Cell(r, 3).Value = x.VariantId ?? "";
                    ws.Cell(r, 4).Value = x.Brand ?? "";
                    ws.Cell(r, 5).Value = x.Description ?? "";
                    ws.Cell(r, 6).Value = x.Category ?? "";
                    ws.Cell(r, 7).Value = x.EcomFlag ?? "Y";
                    ws.Cell(r, 8).Value = x.OnlinePrice.ToString("0.##", CultureInfo.InvariantCulture);

                    ws.Cell(r, 9).Value = x.ShippingWeight ?? "";
                    ws.Cell(r, 10).Value = x.BoxDimA ?? "";
                    ws.Cell(r, 11).Value = x.BoxDimB ?? "";
                    ws.Cell(r, 12).Value = x.BoxDimC ?? "";

                    r++;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(path);
            }, ct);
        }
    }
}
