using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public class Create
    {
        // TODO: implementation 
    }
}