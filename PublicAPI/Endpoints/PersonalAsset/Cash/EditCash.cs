using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.Cash
{
    public class EditCash : EndpointBaseSync.WithoutRequest.WithActionResult<int>
    {
        [Authorize]
        [HttpPut("personalAsset/cash")]
        public override ActionResult<int> Handle()
        {
            throw new System.NotImplementedException();
        }
    }
}