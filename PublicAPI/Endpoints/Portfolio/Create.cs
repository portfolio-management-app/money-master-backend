using ApplicationCore.PortfolioAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    [Authorize]
    [Route("portfolio")]
    public class Create : EndpointBaseSync.WithRequest<CreatePortfolioRequest>.WithActionResult<CreatePortfolioResponse>
    {
        private readonly IPortfolioService _portfolioService;

        public Create(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpPost]
        public override ActionResult<CreatePortfolioResponse> Handle(CreatePortfolioRequest request)
        {
            var userId = (int)HttpContext.Items["userId"]!;

            var newPortfolio
                = _portfolioService.CreatePortfolio(userId, request.Name, request.InitialCash, request.InitialCurrency);

            return Ok(newPortfolio.Adapt<CreatePortfolioResponse>());
        }
    }
}