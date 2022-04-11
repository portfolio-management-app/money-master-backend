namespace ApplicationCore.AssetAggregate.StockAggregate.DTOs
{
    public class StockPriceDto
    {
        public decimal CurrentPrice { get; set; } // c
        public decimal Change { get; set; } // d
        public decimal PercentChange { get; set; } // dp
        public decimal HighPriceOfTheDay { get; set; } //h
        public decimal LowPriceOfTheDay { get; set; } //l
        public decimal OpenPriceOfTheDay { get; set; } // o
        public decimal PreviousClosePrice { get; set; } // pc 
    }
}