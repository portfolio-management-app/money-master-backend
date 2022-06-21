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
    public class SetRead : EndpointBaseSync.WithRequest<SetReadRequest>.WithActionResult<
        GetListUserNotificationResponse>
    {
        private readonly IUserNotificationService _userNotificationService;

        public SetRead(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }

        [HttpPut]
        public override ActionResult<GetListUserNotificationResponse> Handle(SetReadRequest request)
        {
            var result = _userNotificationService.ReadUserNotification(request.id);
            if (result is null) return NotFound();
            return Ok(result.Adapt<GetListUserNotificationResponse>());
        }
    }
}