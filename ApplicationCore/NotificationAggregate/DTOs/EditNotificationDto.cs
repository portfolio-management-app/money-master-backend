using System;

namespace ApplicationCore.NotificationAggregate.DTOs
{
    public class EditNotificationDto
    {
        public decimal HighThreadHoldAmount { get; set; }

        public decimal LowThreadHoldAmount { get; set; }

        public bool IsHighOn { get; set; } = true;

        public bool IsLowOn { get; set; } = true;
    }
}