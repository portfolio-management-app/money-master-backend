using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public abstract class
        BaseRealEstateEndpoint<TRequest, TResponse> : EndpointBaseAsync.WithRequest<TRequest>.WithActionResult<
            TResponse>
    {
        protected readonly IRealEstateService RealEstateService;
        private IAuthorizationService _authorizationService;

        protected int? CurrentUserId =>
            (int)HttpContext.Items["userId"]!;

        protected async Task<bool> IsAllowedToExecute(int portfolioId)
        {
            var authResult =
                await _authorizationService
                    .AuthorizeAsync(User, portfolioId, "CanAccessPortfolioSpecificContent");
            return authResult.Succeeded;
        }


        public BaseRealEstateEndpoint(IRealEstateService realEstateService, IAuthorizationService authorizationService)
        {
            RealEstateService = realEstateService;
            _authorizationService = authorizationService;
        }
    }
}