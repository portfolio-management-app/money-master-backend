using System;

namespace PublicAPI.UserNotification
{
    public class GetListUserNotificationResponse
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public int AssetId { get; set; }

        public int PortfolioId { get; set; }

        public string AssetName { get; set; }

        public string AssetType { get; set; }

        public string Currency { get; set; }

        public decimal HighThreadHoldAmount { get; set; }

        public decimal LowThreadHoldAmount { get; set; }

        public string NotificationType { get; set; }

        public bool IsRead { get; set; } = false;
    }
}