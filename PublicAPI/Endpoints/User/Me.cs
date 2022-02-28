using System.Linq;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.User
{
    public class Me: EndpointBaseSync.WithoutRequest.WithActionResult<MeResponse>
    {
        private readonly IUserService _userService;
        public Me(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet("user/me")]
        public override ActionResult<MeResponse> Handle()
        {
            var userId = (int) HttpContext.Items["userId"]!;
            var foundUser = _userService.GetUserById(userId);
            if (foundUser is null)
                return NotFound();
            return Ok(foundUser.Adapt<MeResponse>());
        }
        
    }
}