using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PublicAPI.Endpoints.Account
{
    [Route("account")]
    public class VerifyOTP : EndpointBaseAsync.WithRequest<VerifyOTPRequest>.WithActionResult<
        string>
    {
        private readonly IUserService _userService;


        public VerifyOTP(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("otp")]
        public override async Task<ActionResult<string>> HandleAsync(VerifyOTPRequest request,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var result = _userService.VerifyOtpCode(request.Email, request.OtpCode);
                if (!result) throw new ApplicationException("OTP not correct");

                return Ok("OTP verified");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}