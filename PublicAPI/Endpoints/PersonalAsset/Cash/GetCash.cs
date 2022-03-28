using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.Cash
{
    public class GetCash : EndpointBaseSync.WithoutRequest.WithActionResult<int>
    {
        [HttpGet("personalAsset/cash")]
        public override ActionResult<int> Handle()
        {
            throw new System.NotImplementedException();
        }
    }
}