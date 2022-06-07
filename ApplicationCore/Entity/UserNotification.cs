using System;
using System.Collections.Generic;

namespace ApplicationCore.Entity
{
    public class UserNotification : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

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