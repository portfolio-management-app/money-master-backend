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

    public class EditNotification : BasePortfolioRelatedEndpoint<EditNotificationRequest, RegisterNotificationResponse>

    {
        private readonly INotificationService _notificationService;

        public EditNotification(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPut("notification/{notificationId}")]
        public override async Task<ActionResult<RegisterNotificationResponse>> HandleAsync([FromMultipleSource] EditNotificationRequest request, CancellationToken cancellationToken = new())
        {
            var dto = request.EditNotificationCommand.Adapt<EditNotificationDto>();
            var result = _notificationService.EditNotification(request.NotificationId, dto);
            return Ok(result.Adapt<RegisterNotificationResponse>());
        }
    }
}