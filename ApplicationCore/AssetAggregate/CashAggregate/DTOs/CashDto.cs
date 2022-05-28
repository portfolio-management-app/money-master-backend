using System;

namespace ApplicationCore.AssetAggregate.CashAggregate.DTOs
{
    public class CashDto
    {
        public string CurrencyCode { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public string Description { get; set; }
        
        public bool IsUsingInvestFund { get; set; }
        public bool IsUsingCash { get; set; }
        public int? UsingCashId { get; set; }
    }
}