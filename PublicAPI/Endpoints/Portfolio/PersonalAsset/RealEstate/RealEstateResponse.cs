using System;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class RealEstateResponse
    {
        public int Id { get; set; }

        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; } = DateTime.Now;
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsDeleted { get; set; }
    }
}