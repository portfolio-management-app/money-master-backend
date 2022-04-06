using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.Stock
{
    public class CreateNewStockCommand  {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public string MarketCode { get; set; } // NYSE, HOSE
    }
    public class CreateNewStockRequest
    {
        [FromRoute] public int PortfolioId { get; set; }

        [FromBody]
        public CreateNewStockCommand CreateNewStockCommand
        {
            get;
            set;
        }
    }
}