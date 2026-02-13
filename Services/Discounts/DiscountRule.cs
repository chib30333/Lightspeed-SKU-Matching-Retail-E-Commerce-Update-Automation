namespace Dupont_Price_Lists.Services.Discounts
{
    public sealed class DiscountRule
    {
        // All keys that can identify this rule row (e.g., Brand, Vendor, Lyncar)
        public List<string> Keys { get; set; } = new();

        public string? TagContains { get; set; }
        public string? SkuStartsWith { get; set; }

        public string? DefaultCostRule { get; set; }
        public string? VendorCostRule { get; set; }
        public string? DefaultPriceRule { get; set; }
        public string? RetailPriceRule { get; set; }
        public string? ContractorPriceRule { get; set; }
        public string? DesignerPriceRule { get; set; }
        public string? OnlinePriceRule { get; set; }
        public string? VipPriceRule { get; set; }
    }
}
