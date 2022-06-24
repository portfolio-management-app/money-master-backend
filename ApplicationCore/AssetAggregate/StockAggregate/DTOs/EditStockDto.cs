using System;

namespace ApplicationCore.AssetAggregate.StockAggregate.DTOs
{
    public class EditStockDto
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public string Description { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public decimal PurchasePrice { get; set; }
        
    }
}