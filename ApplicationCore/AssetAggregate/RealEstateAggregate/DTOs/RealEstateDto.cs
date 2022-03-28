using System;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs
{
    public class RealEstateDto
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public decimal BuyPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Description { get; set; }
    }
}