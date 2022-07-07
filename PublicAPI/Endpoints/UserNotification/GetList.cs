using ApplicationCore.UserNotificationAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PublicAPI.Endpoints.UserNotification;
using ApplicationCore.PortfolioAggregate;

namespace PublicAPI.UserNotification
{
    [Authorize]
    [Route("userNotification")]
    public class GetList : EndpointBaseSync.WithoutRequest.WithActionResult<List<GetListUserNotificationResponse>>
    {
        private readonly IUserNotificationService _userNotificationService;

        private readonly IPortfolioService _portfolioService;

        public GetList(IUserNotificationService userNotificationService, IPortfolioService portfolioService)
        {
            _userNotificationService = userNotificationService;
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public override ActionResult<List<GetListUserNotificationResponse>> Handle()
        {
            var userId = (int)HttpContext.Items["userId"]!;
            var list = _userNotificationService.GetListNotificationByUserId(userId);
            var res = list.Adapt<List<GetListUserNotificationResponse>>();
            res.ForEach(item =>
            {
                item.PortfolioName = _portfolioService.GetPortfolioById(item.PortfolioId).Name;
            });
            return Ok(res);
        }
    }
}