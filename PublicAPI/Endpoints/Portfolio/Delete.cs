using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.PortfolioAggregate;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    public class Delete: BasePortfolioRelatedEndpoint<int, PortfolioResponse>
    {
        private readonly IPortfolioService _portfolioService;

        public Delete(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public override async  Task<ActionResult<PortfolioResponse>> 
            HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = _portfolioService.DeletePortfolio(portfolioId);
            if (result is null)
                return NotFound();

            return result.Adapt<PortfolioResponse>(); 

        }
    }
}