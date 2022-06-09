using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio
{
    [Authorize]
    [Route("/portfolio/{portfolioId}")]
    public abstract class
        BasePortfolioRelatedEndpoint<TRequest, TResponse> : EndpointBaseAsync.WithRequest<TRequest>.WithActionResult<
            TResponse>
    {
        protected int? CurrentUserId =>
            (int)HttpContext.Items["userId"]!;

        protected string NotAllowedPortfolioMessage => "You are not allowed to access this portfolio";


        protected async Task<bool> IsAllowedToExecute(int portfolioId, IAuthorizationService authorizationService)
        {
            var authResult =
                await authorizationService
                    .AuthorizeAsync(User, portfolioId, "CanAccessPortfolioSpecificContent");
            return authResult.Succeeded;
        }

        protected BasePortfolioRelatedEndpoint()
        {
        }
    }
}