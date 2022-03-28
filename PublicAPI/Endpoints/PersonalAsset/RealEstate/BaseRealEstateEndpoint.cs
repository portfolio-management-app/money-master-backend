using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public abstract class
        BaseRealEstateEndpoint<TRequest, TResponse> : EndpointBaseSync.WithRequest<TRequest>.WithActionResult<TResponse>
    {
        protected readonly IRealEstateService RealEstateService;

        protected int? CurrentUser =>
            (int)HttpContext.Items["userId"]!;

        public BaseRealEstateEndpoint(IRealEstateService realEstateService)
        {
            RealEstateService = realEstateService;
        }
    }
}