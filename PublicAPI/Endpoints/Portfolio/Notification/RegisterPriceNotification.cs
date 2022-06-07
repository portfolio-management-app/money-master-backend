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

    public class RegisterHighNotification : BasePortfolioRelatedEndpoint<RegisterNotificationRequest, RegisterNotificationResponse>

    {
        private readonly INotificationService _notificationService;

        public RegisterHighNotification(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("notification")]
        public override async Task<ActionResult<RegisterNotificationResponse>> HandleAsync([FromMultipleSource] RegisterNotificationRequest request, CancellationToken cancellationToken = new())
        {
            var userId = (int)HttpContext.Items["userId"]!;
            var dto = request.RegisterNotificationCommand.Adapt<NotificationDto>();

            var result = _notificationService.RegisterPriceNotification(userId, request.PortfolioId, dto, request.RegisterNotificationCommand.IsHigh);
            return Ok(result.Adapt<RegisterNotificationResponse>());

        }

    }
}