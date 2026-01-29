using DocumentFormat.OpenXml.Drawing;
using Dupont_Price_Lists.Models;
using Dupont_Price_Lists.Models.Dupont_Price_Lists.Output;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Dupont_Price_Lists.Services.FileReaderService;

namespace Dupont_Price_Lists.Services
{

    public sealed class RetailOption
    {
        // Vendor source fields
        public string VendorSkuField { get; set; } = "Item Number";
        public string VendorBrandField { get; set; } = "Brand";
        public string VendorDescField { get; set; } = "Description";
        public string VendorFinishField { get; set; } = "Finish";
        public string VendorMsrpField { get; set; } = "MSRP";
        public string VendorCategoryField { get; set; } = "Category";
        public string VendorCustomSkuField { get; set; } = "Custom SKU";
        public string VendorVendorField { get; set; } = ""; // optional
        public string? VendorCustomTagField { get; set; }  // <- add: where "Clearance"/"Display" might live

        // Lightspeed fields
        public string LsSystemIdField { get; set; } = "System ID";
        public string LsUpcField { get; set; } = "UPC";
        public string LsCustomSkuField { get; set; } = "Custom SKU";
        public string? LsDescriptionField { get; set; } = null;
        public string? LsCustomTagField { get; set; }  // <- add: where "Clearance"/"Display" might live

        // Brand behavior
        public bool UseFixedBrand { get; set; } = false;
        public string FixedBrand { get; set; } = "";
        public bool UseFixedVendor { get; set; } = false;
        public string FixedVendor { get; set; } = "";

        // New-item description template
        public string NewDescriptionTemplate { get; set; } = "{BRAND} - {DESC} - {FINISH} - {SKU}";

        public Func<string, string> KeyNormalizer { get; set; } = MatchRetailOptions.DefaultNormalizer;
    }

    public static class RetailBuilder
    {
        public static List<ItemRecord> BuildRetail(
            MatchResult match,
            FieldMapping mapping,
            RetailOption opt,
            CategoryResolverHierarchical categoryResolver,
            DiscountResolver discountResolver)
        {
            // --- index Found by normalized SKU
            var foundByKey = new Dictionary<string, FoundMatch>(StringComparer.OrdinalIgnoreCase);
            foreach (var f in match.Found)
            {
                var raw = f.Vendor.GetField(mapping.VendorSkuField);
                var key = opt.KeyNormalizer(raw);
                if (!string.IsNullOrEmpty(key)) foundByKey[key] = f;
            }

            var output = new List<ItemRecord>(match.Retail.Count);

            foreach (var rm in match.Retail)
            {
                var vendor = rm.vendorItem;

                var rawSku = vendor.GetField(mapping.VendorSkuField);
                if (string.IsNullOrWhiteSpace(rawSku)) continue;

                var skuKey = opt.KeyNormalizer(rawSku);

                var brand = opt.UseFixedBrand ? opt.FixedBrand : vendor.GetField(opt.VendorBrandField);
                var vendorField = opt.UseFixedVendor ? opt.FixedVendor : vendor.GetField(opt.VendorVendorField);
                var desc = vendor.GetField(opt.VendorDescField);
                var finish = vendor.GetField(opt.VendorFinishField);
                var msrp = PriceMath.ParseDecimal(vendor.GetField(opt.VendorFinishField));

                foreach (var item in vendor.GetFields())
                {
                    if (item.Key.Contains("New List Price") && item.Key.Contains("USD"))
                    {
                        msrp = PriceMath.ParseDecimal(item.Value);
                        break;
                    }
                }

                var d = new ItemRecord();
                d.SetField(RetailFields.ManufactSku, rawSku);
                d.SetField(RetailFields.Brand, brand);
                d.SetField(RetailFields.RecordType, rm.specify ?? "");
                if (!string.IsNullOrWhiteSpace(opt.VendorVendorField))
                    d.SetField(RetailFields.Vendor, opt.VendorVendorField);

                // ---------- Step 3: structure + Found/New specifics ----------
                if (string.Equals(rm.specify, "Found", StringComparison.OrdinalIgnoreCase) &&
                    foundByKey.TryGetValue(skuKey, out var fm))
                {
                    var ls = fm.LightspeedPrimary;

                    brand = opt.UseFixedBrand ? opt.FixedBrand : rm.lightspeedItem.GetField(opt.VendorBrandField);
                    vendorField = opt.UseFixedVendor ? opt.FixedVendor : rm.lightspeedItem.GetField(opt.VendorVendorField);
                    desc = vendor.GetField(opt.VendorDescField);
                    finish = vendor.GetField(opt.VendorFinishField);

                    d.SetField(RetailFields.SystemId, ls.GetField(opt.LsSystemIdField));
                    d.SetField(RetailFields.UPC, ls.GetField(opt.LsUpcField));
                    d.SetField(RetailFields.CustomSku, ls.GetField(opt.LsCustomSkuField));
                    d.SetField(RetailFields.Brand, ls.GetField(opt.VendorBrandField));
                    d.SetField(RetailFields.Vendor, ls.GetField(opt.VendorVendorField));

                    var finalDesc = !string.IsNullOrWhiteSpace(desc)
                        ? desc
                        : (opt.LsDescriptionField is null ? "" : ls.GetField(opt.LsDescriptionField));

                    if (string.IsNullOrWhiteSpace(finalDesc))
                        finalDesc = ExpandTemplate(opt.NewDescriptionTemplate, brand, desc, finish, rawSku);

                    d.SetField(RetailFields.Description, EnsureBrandDash(brand, finalDesc));
                }
                else
                {
                    d.SetField(RetailFields.SystemId, "");
                    d.SetField(RetailFields.UPC, "");
                    d.SetField(RetailFields.CustomSku, "");

                    var finalDesc = ExpandTemplate(opt.NewDescriptionTemplate, brand, desc, finish, rawSku);
                    d.SetField(RetailFields.Description, EnsureBrandDash(brand, finalDesc));
                }

                // ---------- Step 4a: Category (hierarchical) ----------
                string category = vendor.GetField(opt.VendorCategoryField);
                if (string.IsNullOrWhiteSpace(category))
                {
                    category = categoryResolver.Resolve(
                        d,
                        opt.VendorDescField, opt.VendorSkuField, opt.VendorBrandField
                    ) ?? "";
                }
                d.SetField(RetailFields.Category, category);

                // ---------- Step 4b: Discount + pricing ----------
                var rules = discountResolver.ResolveRule(
                    d,
                    brandField: opt.VendorBrandField,
                    skuField: mapping.VendorSkuField,
                    customSkuField: opt.VendorCustomSkuField,
                    opt.VendorDescField, opt.VendorBrandField
                ) ?? new Dictionary<string, string>();

                d.SetField(RetailFields.MSRP, msrp.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));

                foreach (var ruleItem in rules)
                {
                    decimal final;
                    if (vendor.GetField(RetailFields.CustomSku).Contains("Clearance") && !vendor.GetField(RetailFields.CustomSku).Contains("Display"))
                    {
                        var rule = "70 / 10";
                        final = PriceMath.ApplyDiscountChain(msrp, rule);
                    } else if(vendor.GetField(RetailFields.CustomSku).Contains("Display") && !vendor.GetField(RetailFields.CustomSku).Contains("Clearance"))
                    {
                        var rule = "50 / 10";
                        final = PriceMath.ApplyDiscountChain(msrp, rule);
                    } else if(vendor.GetField(RetailFields.CustomSku).Contains("Clearance Display"))
                    {
                        var rule = "60 / 10";
                        final = PriceMath.ApplyDiscountChain(msrp, rule);
                    } else
                    {
                        final = string.IsNullOrEmpty(ruleItem.Value) ? msrp : PriceMath.ApplyDiscountChain(msrp, ruleItem.Value);
                        if (ruleItem.Key.Contains("Default Cost"))
                        {
                            d.SetField(RetailFields.DefaultCost, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("Vendor Cost"))
                        {
                            d.SetField(RetailFields.VendorCost, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("Retail"))
                        {
                            d.SetField(RetailFields.RetailPrice, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("Default Price"))
                        {
                            d.SetField(RetailFields.DefaultPrice, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("Online"))
                        {
                            d.SetField(RetailFields.OnlinePrice, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("Designer"))
                        {
                            d.SetField(RetailFields.DesignerPrice, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("V.I.P"))
                        {
                            d.SetField(RetailFields.VIPPrice, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                        else if (ruleItem.Key.Contains("Contractor") && !ruleItem.Key.Contains("Retail"))
                        {
                            d.SetField(RetailFields.ContractorPrice, final.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture));
                        }
                    }
                }

                output.Add(d);
            }

            // ---------- Step 5: Archive duplicates by lower MSRP ----------
            ArchiveLowerMsrpDuplicates(output);

            return output;
        }

        private static string ExpandTemplate(string tmpl, string brand, string desc, string finish, string sku)
        {
            string S(string s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();
            return tmpl
                .Replace("{BRAND}", S(brand))
                .Replace("{DESC}", S(desc))
                .Replace("{FINISH}", S(finish))
                .Replace("{SKU}", S(sku));
        }

        private static string EnsureBrandDash(string brand, string description)
        {
            var b = (brand ?? "").Trim();
            var d = (description ?? "").Trim();

            if (string.IsNullOrEmpty(b)) return d;
            if (d.StartsWith(b + " - ", StringComparison.OrdinalIgnoreCase)) return d;

            if (d.StartsWith(b + " ", StringComparison.OrdinalIgnoreCase))
                d = d.Substring(b.Length).TrimStart('-', ' ');

            return $"{b} - {d}";
        }

        private static void ArchiveLowerMsrpDuplicates(List<ItemRecord> rows)
        {
            var groups = rows.GroupBy(r => (r.GetField(RetailFields.ManufactSku) ?? "").Trim(),
                                      StringComparer.OrdinalIgnoreCase);

            foreach (var g in groups)
            {
                var list = g.ToList();
                if (list.Count <= 1) continue;

                var ordered = list
                    .Select(r => (r, msrp: PriceMath.ParseDecimal(r.GetField(RetailFields.MSRP))))
                    .OrderByDescending(x => x.msrp)
                    .ToList();

                bool first = true;
                foreach (var x in ordered)
                {
                    x.r.SetField(RetailFields.Archive, first ? "N" : "Y");
                    first = false;
                }
            }
        }
    }
}
