using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Notification
{

    public class EditNotificationCommand
    {
        public decimal HighThreadHoldAmount { get; set; }
        public decimal LowThreadHoldAmount { get; set; }
        public bool IsHighOn { get; set; }

        public bool IsLowOn { get; set; }
    }

    public class EditNotificationRequest
    {
        [FromBody] public EditNotificationCommand EditNotificationCommand { get; set; }
        [FromRoute] public int NotificationId { get; set; }
    }
}