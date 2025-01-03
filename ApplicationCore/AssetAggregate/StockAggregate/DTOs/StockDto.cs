using System;

namespace ApplicationCore.AssetAggregate.StockAggregate.DTOs
{
    public class StockDto
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public string MarketCode { get; set; } // NYSE, HOSE
        public decimal PurchasePrice { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsUsingInvestFund { get; set; }
        public bool IsUsingCash { get; set; }
        public int? UsingCashId { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }
}