namespace Dupont_Price_Lists.Models.Outputs
{
    public sealed class OnlineRow
    {
        public string? SystemId { get; set; }
        public string ManufactSku { get; set; } = "";
        public string? VariantId { get; set; }

        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }

        public string EcomFlag { get; set; } = "Y";
        public decimal OnlinePrice { get; set; }

        public string? ShippingWeight { get; set; }
        public string? BoxDimA { get; set; }
        public string? BoxDimB { get; set; }
        public string? BoxDimC { get; set; }
    }
}
