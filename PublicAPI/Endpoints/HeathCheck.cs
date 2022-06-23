using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints
{
    public class HeathCheck: EndpointBaseSync.WithoutRequest.WithActionResult
    {
        [Route("healthCheck")]
        [HttpGet]
        public override ActionResult Handle()
        {
            return Ok("The service is up and running."); 
        }
    }
}