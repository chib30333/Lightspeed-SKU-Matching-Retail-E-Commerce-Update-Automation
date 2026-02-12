namespace Dupont_Price_Lists.Models.Outputs
{
    public sealed class RetailRow
    {
        public string? SystemId { get; set; }
        public string ManufactSku { get; set; } = "";

        public string? CustomSku { get; set; }
        public string? Upc { get; set; }

        public string? Brand { get; set; }
        public string? Vendor { get; set; }

        public string Description { get; set; } = "";
        public string? Finish { get; set; }
        public string? Category { get; set; }

        public decimal Msrp { get; set; }
        public decimal DefaultCost { get; set; }
        public decimal VendorCost { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal ContractorPrice { get; set; }
        public decimal DesignerPrice { get; set; }
        public decimal OnlinePrice { get; set; }
        public decimal VipPrice { get; set; }

        public bool Archive { get; set; }
        public string RecordType { get; set; } = ""; // Found/New
    }
}
