using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.UserAggregate;
using Mapster;

namespace PublicAPI.Endpoints.User
{
    public class Register: EndpointBaseAsync.WithRequest<RegisterRequest>.WithActionResult<RegisterResponse>
    {
        private readonly IUserService _userService;

        public Register( IUserService userService)
        {
            _userService = userService;
        }
        
        
        [HttpPost("user")]
        public override async Task<ActionResult<RegisterResponse>> HandleAsync(RegisterRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var newUser = 
                    await Task.Run(() => _userService.AddNewUser(request.Email, request.Password), cancellationToken);
                return Ok(newUser.Adapt<RegisterResponse>()); 
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}