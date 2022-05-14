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
    
    public class ExternalAuthentication: 
        EndpointBaseAsync.WithRequest<ExternalAuthenticationRequest>.WithActionResult<AuthenticationResponse>
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public ExternalAuthentication(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        
        
        [HttpPost("authentication/google")]
        public override async Task<ActionResult<AuthenticationResponse>> HandleAsync([FromBody]ExternalAuthenticationRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var user = await _userService.TryGoogleAuthentication(request.ExternalToken);
                var generatedToken = 
                                    user.GenerateToken(_configuration["JWTSigningKey"]);
                var response = user.Adapt<AuthenticationResponse>();
                response.Token = generatedToken;
                return Ok(response); 
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message); 
            }
        }
    }

}