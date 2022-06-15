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
    public class SendOTP : EndpointBaseAsync.WithRequest<ForgetPasswordRequest>.WithActionResult<
        ForgetPasswordResponse>
    {
        private readonly IUserService _userService;

        private readonly IEmailSender _emailSender;


        public SendOTP(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }


        [HttpPost("forgetPassword")]
        public override async Task<ActionResult<ForgetPasswordResponse>> HandleAsync(ForgetPasswordRequest request,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var existedUser = _userService.GetUserByEmail(request.Email);
                if (existedUser is null)
                {
                    throw new ApplicationException("Email not exist");
                }
                var random = new Random();
                var otp = random.Next(100000, 999999).ToString();
                await _emailSender.SendEmail(request.Email, MessageBuilder.BuildSubject(request.Lang, request.Email), MessageBuilder.BuildHtmlMessage(request.Lang, otp));
                _userService.AddNewOtp(request.Email, otp);
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