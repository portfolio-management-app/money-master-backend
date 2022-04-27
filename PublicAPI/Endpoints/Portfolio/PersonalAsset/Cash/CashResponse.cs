using System;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class CashResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public DateTime LastChanged { get; set; }
        public int PortfolioId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}