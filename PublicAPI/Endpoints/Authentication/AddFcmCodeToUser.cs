using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Authentication
{
    [Authorize]
    [Route("user/me")]
    public class AddFcmCodeToUser: EndpointBaseAsync.WithRequest<string>.WithActionResult<object>
    {
        private readonly IUserService _userService;

        public AddFcmCodeToUser(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("fcm")]
        public override async Task<ActionResult<object>> HandleAsync([FromBody]string fcmCode, CancellationToken cancellationToken = new CancellationToken())
        {
            int userId = (int)HttpContext.Items["userId"]; 
            var result = _userService.AddFcmCode(userId, fcmCode);
            return Ok();
        }
    }
}