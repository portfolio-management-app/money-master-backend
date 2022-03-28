using System;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    public class RealEstateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; } = DateTime.Now;
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}