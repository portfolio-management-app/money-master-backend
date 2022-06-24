using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class EditStockCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
    }
    public class EditStockRequest
    {
        [FromBody] public EditStockCommand EditStockCommand { get; set; }
        [FromRoute] public int AssetId { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}