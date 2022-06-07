using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Notification
{

    public class RegisterNotificationCommand
    {
        public int AssetId { get; set; }

        public string AssetName { get; set; }

        public string AssetType { get; set; }

        public string Currency { get; set; }

        public decimal HighThreadHoldAmount { get; set; }

        public decimal LowThreadHoldAmount { get; set; }

        public string CoinCode { get; set; }

        public string StockCode { get; set; }

        public bool IsHigh { get; set; }

    }

    public class RegisterNotificationRequest
    {
        [FromBody] public RegisterNotificationCommand RegisterNotificationCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}