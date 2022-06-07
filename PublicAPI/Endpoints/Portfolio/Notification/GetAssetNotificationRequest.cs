using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Notification
{
    public class GetAssetNotificationRequest
    {
        [FromRoute] public int AssetId { get; set; }
        [FromQuery] public string AssetType { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}