using System;

namespace PublicAPI.Endpoints.PersonalAsset.CryptoCurrency
{
    public class CryptoCurrencyResponse
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; } 
        public decimal CurrentAmountHolding { get; set; }
        public DateTime LastChanged { get; set; } 
        public int PortfolioId { get; set; }
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string CryptoCoinCode { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}