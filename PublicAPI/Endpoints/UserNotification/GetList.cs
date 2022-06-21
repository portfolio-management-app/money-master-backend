using ApplicationCore.UserAggregate;
using ApplicationCore.UserNotificationAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PublicAPI.UserNotification
{
    [Authorize]
    [Route("userNotification")]
    public class GetList : EndpointBaseSync.WithoutRequest.WithActionResult<List<GetListUserNotificationResponse>>
    {
        private readonly IUserNotificationService _userNotificationService;

        public GetList(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }

        [HttpGet]
        public override ActionResult<List<GetListUserNotificationResponse>> Handle()
        {
            var userId = (int)HttpContext.Items["userId"]!;
            var list = _userNotificationService.GetListNotificationByUserId(userId);
            return Ok(list.Adapt<List<GetListUserNotificationResponse>>());
        }
    }
}