using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PublicAPI.Endpoints.Account
{
    [Route("account")]
    public class ResetPassword : EndpointBaseAsync.WithRequest<ResetPasswordRequest>.WithActionResult<
        ForgetPasswordResponse>
    {
        private readonly IUserService _userService;



        public ResetPassword(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPut("resetPassword")]
        public override async Task<ActionResult<ForgetPasswordResponse>> HandleAsync(ResetPasswordRequest request,
            CancellationToken cancellationToken = new())
        {
            try
            {
                _userService.SetPassword(request.Email, request.NewPassword);
                var response = new ForgetPasswordResponse();
                response.Email = request.Email;
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}