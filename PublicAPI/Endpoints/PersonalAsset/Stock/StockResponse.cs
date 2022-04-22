using System;

namespace PublicAPI.Endpoints.PersonalAsset.Stock
{
    public class StockResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public DateTime LastChanged { get; set; }
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public string MarketCode { get; set; } // NYSE, HOSE
        public decimal CurrentPrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentAmountInCurrency => CurrentPrice * CurrentAmountHolding;
    }
}