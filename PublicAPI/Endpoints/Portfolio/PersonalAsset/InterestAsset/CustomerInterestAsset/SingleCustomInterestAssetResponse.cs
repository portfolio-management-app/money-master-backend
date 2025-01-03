using System;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class SingleCustomInterestAssetResponse
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day    
        public bool IsDeleted { get; set; }
        public decimal Fee { get; set; }
        public decimal Tax { get; set; }
    }
}