using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Notification
{
    public class RegisterNotificationResponse
    {
        public int Id { get; set; }
        public int AssetId { get; set; }

        public int PortfolioId { get; set; }

        public string AssetName { get; set; }

        public string AssetType { get; set; }

        public string Currency { get; set; }

        public decimal HighThreadHoldAmount { get; set; }

        public decimal LowThreadHoldAmount { get; set; }

        public string CoinCode { get; set; }

        public string StockCode { get; set; }

        public bool IsHighOn { get; set; }

        public bool IsLowOn { get; set; }
    }
}