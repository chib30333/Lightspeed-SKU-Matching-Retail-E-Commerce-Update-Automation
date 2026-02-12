using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Dupont_Price_Lists;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Dupont_Price_Lists.Output;
using Dupont_Price_Lists.Services;
using static Dupont_Price_Lists.Services.FileWriterService;
using static Dupont_Price_Lists.Services.OnlineBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dupont_Price_Lists.Forms
{
    public static class FormProcess
    {
        private static MatchResult? matchResult;
        private static readonly List<ItemRecord>? matchOnlineList;
        private static readonly string? savePath = "save/";

        public static async void FormProcess_Load_From_XSLX(
            ProgressBar progress,
            List<ItemRecord> vendorRows, 
            List<ItemRecord> lightspeedRows, 
            string vendorFilePath, 
            string lightspeedFilePath, 
            FieldMapping fieldMapping, 
            List<string> vendorHeaders, 
            List<string> lightspeedHeaders, 
            string brand,
            string vendor,
            string categoryPath,
            string masterDiscountListPath,
            string specify
        )
        {
            try
            {
                progress.Style = ProgressBarStyle.Marquee;
                //Status.Text = "Matching items ...";

                if(specify == "retail")
                {
                    matchResult = await Task.Run(() =>
                    {
                        return MatchRetailService.MatchItems(vendorRows, lightspeedRows, fieldMapping, new MatchRetailOptions { IgnoreVendorDuplicates = true });
                    });

                    //Status.Text = $"Matched. Found: {matchResult.Found.Count}, New: {matchResult.NewItems.Count}";

                    progress.Style = ProgressBarStyle.Continuous;
                    progress.Value = 100;

                    ///////other actions
                    ///

                    var retailOpt = new RetailOption
                    {
                        VendorSkuField = fieldMapping.LightspeedSkuField,
                        VendorBrandField = "Brand",
                        VendorDescField = "Description",
                        VendorFinishField = "Finish",
                        VendorMsrpField = "MSRP",
                        VendorCategoryField = "Category",   // vendor-supplied (optional)
                        VendorCustomSkuField = "Custom SKU",
                        VendorVendorField = vendor,  // optional fixed Vendor label in output

                        LsSystemIdField = "System ID",
                        LsUpcField = "UPC",
                        LsCustomSkuField = "Custom SKU",
                        LsDescriptionField = null,         // set to "Description" if you exported it from LS

                        UseFixedBrand = brand is "" or null ? false : true,         // set true if price list is single-brand
                        FixedBrand = brand is not "" and not null ? brand : null,

                        UseFixedVendor = vendor is "" or null ? false : true,
                        FixedVendor = vendor is not "" and not null ? vendor : null,

                        KeyNormalizer = MatchRetailOptions.DefaultNormalizer,
                    };

                    var categoryResolver = new CategoryResolverHierarchical(
                        xlsxPath: categoryPath,
                        sheetName: null,      // first sheet
                        separator: " > ",     // how you want the path to look
                        normalizer: MatchRetailOptions.DefaultNormalizer
                    );

                    var discountResolver = new DiscountResolver(
                        xlsxPath: masterDiscountListPath,
                        sheetName: null,          // first sheet
                        brandColumn: "Brand",     // adjust to your columns
                        unit: "USD",   // e.g., "50/10"
                        tagContainsColumn: "TagContains",       // null if you don't have this
                        skuStartsWithColumn: "SkuStartsWith",   // null if you don't have this
                        normalizer: MatchRetailOptions.DefaultNormalizer
                    );

                    // 3) Build File D rows (Step3 + Step4 + Step5 in one go)
                    var RetailRows = await Task.Run(() =>
                    {
                        return RetailBuilder.BuildRetail(
                            matchResult,
                            fieldMapping,
                            retailOpt,
                            categoryResolver,
                            discountResolver
                        );
                    });

                    // 4) Export (CSV or XLSX) – reuse the writers I gave earlier
                    var columns = new List<string>
                    {
                        RetailFields.SystemId, RetailFields.ManufactSku, RetailFields.CustomSku, RetailFields.UPC,
                        RetailFields.Brand, RetailFields.Vendor, RetailFields.Description, RetailFields.Finish, RetailFields.Category,
                        RetailFields.MSRP, RetailFields.DefaultCost, RetailFields.VendorCost, RetailFields.DefaultPrice,
                        RetailFields.RetailPrice, RetailFields.ContractorPrice, RetailFields.DesignerPrice, RetailFields.OnlinePrice, RetailFields.VIPPrice,
                        RetailFields.Archive,
                        RetailFields.RecordType
                    };

                    ////////////////////

                    var saveFound = savePath + "Found.xlsx";
                    var saveNew = savePath + "New.xlsx";
                    var report = savePath + "Report.xlsx";
                    var retail = savePath + "Retail.xlsx";

                    ExcelWriterService.SaveFound(saveFound, matchResult.Found, vendorHeaders, fieldMapping.LightspeedSystemIdField);
                    ExcelWriterService.SaveNew(saveNew, matchResult.NewItems, vendorHeaders);
                    ExcelWriterService.SaveReport(report, matchResult);
                    ExcelWriterService.SaveNew(retail, RetailRows, columns);

                    MessageBox.Show($"Exported:\n{saveFound}\n{saveNew}\n{report}\n{retail}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                } else if(specify == "online")
                {
                    var discountResolver = new DiscountResolver(
                        xlsxPath: masterDiscountListPath,
                        sheetName: null,          // first sheet
                        brandColumn: "Brand",     // adjust to your columns
                        unit: "USD",   // e.g., "50/10"
                        tagContainsColumn: "TagContains",       // null if you don't have this
                        skuStartsWithColumn: "SkuStartsWith",   // null if you don't have this
                        normalizer: MatchRetailOptions.DefaultNormalizer
                    );

                    var OnlineRows = await Task.Run(() =>
                    {
                        return OnlineBuilder.BuildAndValidateFileE(
                            matchResult,
                            vendorRows,
                            fieldMapping,
                            MatchRetailOptions.DefaultNormalizer,
                            discountResolver,
                            out OnlineReport report
                        );
                    });

                    var online = savePath + "Online.xlsx";

                    var columns = new List<string>
                    {
                        OnlineFields.SystemId, OnlineFields.ManufactSku, OnlineFields.VariantId, OnlineFields.Brand,
                        OnlineFields.Description, OnlineFields.Category, OnlineFields.EcomFlag, OnlineFields.OnlinePrice,
                        OnlineFields.ShippingWeight, OnlineFields.BoxDimA, OnlineFields.BoxDimB, OnlineFields.BoxDimC,
                    };

                    ExcelWriterService.SaveNew(online, OnlineRows, columns);

                    MessageBox.Show($"Exported:\n{online}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Processing Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CreateUpdateFiles(List<ItemRecord> matchedItems, Dictionary<string, string> priceMappings, List<string> headers, string filePath)
        {
            foreach (var matchedItem in matchedItems)
            {
                var retailPrice = CalculatePrice(matchedItem, "Retail"); 
                var ecomPrice = CalculatePrice(matchedItem, "Ecom");

                matchedItem.SetField(priceMappings["RetailField"], retailPrice.ToString("F2"));
                matchedItem.SetField(priceMappings["EcomField"], ecomPrice.ToString("F2"));
            }

            ExcelWriterService.SaveUpdatedFile(matchedItems, headers, priceMappings, filePath);
        }
        public static decimal CalculatePrice(ItemRecord item, string channel)
        {
            decimal listPrice = 0m;

            if (!decimal.TryParse(item.GetField("ListPrice"), out listPrice))
                return 0m;

            string stackedDiscount = item.GetField("Discount");
            string type = item.GetField("Type");

            decimal discountedPrice = PricingService.ApplyStackedDiscount(listPrice, stackedDiscount);

            switch (channel)
            {
                case "Retail":
                    discountedPrice = PricingService.ApplyRetailRules(discountedPrice, type);
                    break;

                case "Ecom":
                    discountedPrice = PricingService.ApplyEcomRules(discountedPrice, type);
                    break;
            }

            return Math.Round(discountedPrice, 2, MidpointRounding.AwayFromZero);
        }

    }
}