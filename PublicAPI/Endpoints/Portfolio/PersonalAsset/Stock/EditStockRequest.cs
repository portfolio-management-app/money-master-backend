using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class EditStockCommand
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public decimal PurchasePrice { get; set; }
    }
    public class EditStockRequest
    {
        [FromBody] public EditPortfolioCommand EditPortfolioCommand { get; set; }
        [FromRoute] public int AssetId { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}