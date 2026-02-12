using System.Collections.Generic;

namespace Dupont_Price_Lists.Models
{
    public sealed class MappingProfile
    {
        public string ProfileName { get; set; } = "Default";

        // Vendor file (File C)
        public string VendorSkuField { get; set; } = "";
        public string? VendorUpcField { get; set; }
        public string? VendorPriceField { get; set; }
        public string? VendorDescriptionField { get; set; }
        public string? VendorFinishField { get; set; }
        public string? VendorWeightField { get; set; }
        public string? VendorDimensionsField { get; set; }
        public string? VendorBrandField { get; set; }

        // Lightspeed file (File A)
        public string LightspeedSkuField { get; set; } = "Manufact SKU";
        public string LightspeedSystemIdField { get; set; } = "System ID";
        public string LightspeedCustomSkuField { get; set; } = "Custom SKU";
        public string LightspeedUpcField { get; set; } = "UPC";
        public string LightspeedMsrpField { get; set; } = "MSRP";
        public string LightspeedEcomField { get; set; } = "Ecom"; // adjust if your export uses another header

        // Online file (File B)
        public string OnlineSkuField { get; set; } = "Manufact SKU";

        // Brand/Vendor strategy
        public bool UseFixedBrand { get; set; }
        public string? FixedBrand { get; set; }
        public bool UseBrandFromField { get; set; }
        public bool UseFixedVendor { get; set; }
        public string? FixedVendor { get; set; }

        // New item description template
        public string NewDescriptionTemplate { get; set; } = "{BRAND} - {DESC} - {FINISH} - {SKU}";

        // Category
        public string CategorySeparator { get; set; } = " > ";
        public List<string> CategoryScanFields { get; set; } = new() { "Description", "Finish", "Manufact SKU" };

        // Normalization options
        public bool UppercaseSku { get; set; } = true;
        public bool TrimSkuWhitespace { get; set; } = true;
    }
}
