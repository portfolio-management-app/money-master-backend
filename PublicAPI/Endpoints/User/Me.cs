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
        public override ActionResult<MeResponse> Handle()
        {
            int userId = GetUserIdFromToken();
            var foundUser = _userService.GetUserById(userId);
            if (foundUser is null)
                return NotFound();
            return Ok(foundUser.Adapt<MeResponse>());
        }

        private int GetUserIdFromToken()
        {
            var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == "ID").Value);
            return userId;
        }
        
    }
}