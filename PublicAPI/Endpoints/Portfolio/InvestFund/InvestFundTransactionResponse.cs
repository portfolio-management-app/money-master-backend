using System;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class InvestFundTransactionResponse
    {
        public int Id { get; set; }
        public int ReferentialAssetId { get; set; }
        public string ReferentialAssetType { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public int InvestFundId { get; set; }
        public bool IsIngoing { get; set; }
    }
}