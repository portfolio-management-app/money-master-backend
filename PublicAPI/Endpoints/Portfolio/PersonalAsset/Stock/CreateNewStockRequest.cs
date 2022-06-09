using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class CreateNewStockCommand : BaseCreateRequest
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public string MarketCode { get; set; } // NYSE, HOSE
        public decimal PurchasePrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }

    public class CreateNewStockRequest
    {
        [FromRoute] public int PortfolioId { get; set; }

        [FromBody] public CreateNewStockCommand CreateNewStockCommand { get; set; }
    }
}