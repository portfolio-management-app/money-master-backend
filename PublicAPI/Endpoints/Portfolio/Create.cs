using System.Net.Http;
using ApplicationCore.InvestFundAggregate;
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
        private readonly IInvestFundService _investFundService;

        public Create(IPortfolioService portfolioService, IInvestFundService investFundService)
        {
            _portfolioService = portfolioService;
            _investFundService = investFundService;
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