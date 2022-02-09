using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.UserAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace PublicAPI.Endpoints.User
{
    public class Register: EndpointBaseAsync.WithRequest<RegisterRequest>.WithActionResult<RegisterResponse>
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public Register( IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        
        
        [HttpPost("user")]
        public override async Task<ActionResult<RegisterResponse>> HandleAsync(RegisterRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var newUser = 
                    await Task.Run(() => _userService.AddNewUser(request.Email, request.Password), cancellationToken);
                string token = newUser.GenerateToken(_configuration["JWTSigningKey"]);
                var response = newUser.Adapt<RegisterResponse>();
                response.Token = token;
                return Ok(response); 
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}