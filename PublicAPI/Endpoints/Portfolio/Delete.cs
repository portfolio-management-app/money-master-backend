using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.PortfolioAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    public class Delete: BasePortfolioRelatedEndpoint<int, PortfolioResponse>
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IAuthorizationService _authorizationService;

        public Delete(IPortfolioService portfolioService, IAuthorizationService authorizationService)
        {
            _portfolioService = portfolioService;
            _authorizationService = authorizationService;
        }
        
        [HttpDelete]
        public override async  Task<ActionResult<PortfolioResponse>> 
            HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
            {
                return Unauthorized(); 
            }
            var result = _portfolioService.DeletePortfolio(portfolioId);
            if (result is null)
                return NotFound();

            return result.Adapt<PortfolioResponse>(); 

        }
    }
}