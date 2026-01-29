using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Models
{
    namespace Dupont_Price_Lists.Output
    {
        public static class RetailFields
        {
            public const string SystemId = "System ID";
            public const string ManufactSku = "Manufact SKU";
            public const string CustomSku = "Custom SKU";
            public const string UPC = "UPC";
            public const string Brand = "Brand";
            public const string Vendor = "Vendor";
            public const string Description = "Description";
            public const string Finish = "Finish";
            public const string Category = "Category";

            public const string MSRP = "MSRP";
            public const string DefaultCost = "Default Cost";
            public const string VendorCost = "Vendor Cost";
            public const string DefaultPrice = "Default Price";
            public const string RetailPrice = "Retail Price";
            public const string ContractorPrice = "Contractor Price";
            public const string DesignerPrice = "Designer Price";
            public const string OnlinePrice = "Online Price";
            public const string VIPPrice = "V.I.P Price";

            //public const string Ecom = "Ecom";
            //public const string VariantId = "Variant ID";
            //public const string ShipWeight = "Shipping Weight";
            //public const string ShipBoxA = "Shipping Box A";
            //public const string ShipBoxB = "Shipping Box B";
            //public const string ShipBoxC = "Shipping Box C";

            public const string Archive = "Archive"; // "Y" or "N"
            public const string RecordType = "Record Type"; // "Found"/"New" (for your visibility)
        }
    }

}
