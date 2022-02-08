using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Authentication
{
    public class Authentication: EndpointBaseAsync.WithRequest<AuthenticationRequest>.WithActionResult<AuthenticationResponse>
    {
        [HttpPost( "authentication")]
        public override async Task<ActionResult<AuthenticationResponse>> HandleAsync(AuthenticationRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.Run(() => Ok(request), cancellationToken);
        }
    }
}