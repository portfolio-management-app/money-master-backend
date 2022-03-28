using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PublicAPI.Endpoints.Authentication
{
    public class Authentication : EndpointBaseAsync.WithRequest<AuthenticationRequest>.WithActionResult<
        AuthenticationResponse>
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public Authentication(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("authentication")]
        public override async Task<ActionResult<AuthenticationResponse>> HandleAsync(AuthenticationRequest request,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var authenticatedUser = _userService.TryAuthenticate(request.Email, request.Password);
                if (authenticatedUser is null)
                    return Unauthorized("Credential failed");
                var jwtToken = authenticatedUser
                    .GenerateToken(_configuration["JWTSigningKey"]);

                var response = authenticatedUser.Adapt<AuthenticationResponse>();
                response.Token = jwtToken;

                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}