using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dupont_Price_Lists.Models
{
    public static class OnlineFields
    {
        public const string SystemId = "System ID";
        public const string ManufactSku = "Manufact SKU";
        public const string VariantId = "Variant ID";

        // Shipping / logistics
        public const string ShippingWeight = "Shipping Weight";
        public const string BoxDimA = "Shipping Box Dimensions A";
        public const string BoxDimB = "Shipping Box Dimensions B";
        public const string BoxDimC = "Shipping Box Dimensions C";

        // Commerce flags / prices
        public const string EcomFlag = "Ecom";        // "Y" or "N" (from File A)
        public const string OnlinePrice = "Online Price";

        // Optional but handy to keep consistent context
        public const string Description = "Description";
        public const string Brand = "Brand";
        public const string Category = "Category";
    }
}
