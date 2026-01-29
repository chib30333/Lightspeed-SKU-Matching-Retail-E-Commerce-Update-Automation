using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Models
{
    public class FieldMapping
    {
        public string? VendorSkuField { get; set; }
        public string? VendorPriceField { get; set; }
        public string? VendorDescriptionField { get; set; }
        public string? VendorWeightField { get; set; }
        public string? VendorDimensionsField { get; set; }
        public string? VendorUpcField { get; set; }
        public string? VendorBrandField { get; set;}

        public string LightspeedSkuField { get; set; } = "Manufact SKU";
        public string LightspeedSystemIdField { get; set; } = "System ID";
        public string LightspeedPriceField { get; set; } = "Price";
        public string LightspeedRetailField { get; set; } = "Retail Price";
        public string LightspeedEcomField { get; set; } = "Publish to eCom";
        public string? LightspeedMsrpField { get; set; } = "MSRP";
        public string LightspeedCustomSkuField { get; set; } = "Custom SKU";
    }
}
