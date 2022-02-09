using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.UserAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Authentication
{
    public class Authentication: EndpointBaseAsync.WithRequest<AuthenticationRequest>.WithActionResult<AuthenticationResponse>
    {
        private readonly IUserService _userService;
        public Authentication(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost( "authentication")]
        public override async Task<ActionResult<AuthenticationResponse>> HandleAsync(AuthenticationRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var authenticatedUser = _userService.TryAuthenticate(request.Email, request.Password);
                if (authenticatedUser is null)
                    return Unauthorized("Credential failed");
                var jwtToken = authenticatedUser
                    .GenerateToken("3aacfb02-b67b-4923-8a2d-21a103902b91");
                
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