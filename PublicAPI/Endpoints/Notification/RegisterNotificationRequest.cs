namespace PublicAPI.Endpoints.Notification
{
    public class RegisterNotificationRequest
    {
        public int AssetId { get; set; }

        public int PortfolioId { get; set; }

        public string AssetName { get; set; }

        public string AssetType { get; set; }

        public string Currency { get; set; }

        public string CoinCode { get; set; } = null;

        public string StockCode { get; set; } = null;

        public double ThreadHoldAmount { get; set; }

    }
}