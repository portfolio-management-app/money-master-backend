using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.NotificationAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.NotificationAggregate.DTOs;
using PublicAPI.Attributes;


namespace PublicAPI.Endpoints.Portfolio.Notification
{

    public class GetNotificationAsset : BasePortfolioRelatedEndpoint<GetAssetNotificationRequest, RegisterNotificationResponse>

    {
        private readonly INotificationService _notificationService;

        public GetNotificationAsset(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("notification/asset/{assetId}")]
        public override async Task<ActionResult<RegisterNotificationResponse>> HandleAsync([FromMultipleSource] GetAssetNotificationRequest request, CancellationToken cancellationToken = new())
        {
            var userId = (int)HttpContext.Items["userId"]!;

            var result = _notificationService.GetNotificationByAssetIdAndAssetType(request.AssetId, userId, request.PortfolioId, request.AssetType);
            if (result is null)
                return NotFound();
            return Ok(result.Adapt<RegisterNotificationResponse>());
        }

    }
}