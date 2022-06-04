using System;
using System.Net.Http;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Notification
{
    [Authorize]
    [Route("notification")]
    public class RegisterNotification
    {
        [HttpPost]
        public ActionResult<string> Handle(RegisterNotificationRequest request)
        {
            return "";
        }
    }
}