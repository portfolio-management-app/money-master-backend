using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace PublicAPI.Endpoints.Account
{
    [Authorize]
    [Route("account")]
    public class UpdatePassword : EndpointBaseAsync.WithRequest<UpdatePasswordRequest>.WithActionResult<string>
    {
        private readonly IUserService _userService;


        public UpdatePassword(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("password")]
        public override async Task<ActionResult<string>> HandleAsync(UpdatePasswordRequest request,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var userId = (int)HttpContext.Items["userId"];
                _userService.UpdatePassword(userId, request.NewPassword, request.OldPassword);

                return Ok("Update password success");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}