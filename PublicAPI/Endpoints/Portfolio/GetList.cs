using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.PortfolioAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    [Authorize]
    [Route("portfolio")]
    public class GetList : EndpointBaseSync.WithoutRequest.WithActionResult<List<GetListPortfolioResponse>>
    {
        private readonly IPortfolioService _portfolioService;

        public GetList(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public override ActionResult<List<GetListPortfolioResponse>> Handle()
        {
            var userId = (int)HttpContext.Items["userId"]!;
            var listPortfolio = _portfolioService.GetPortfolioList(userId);

            return Ok(listPortfolio.Adapt<List<GetListPortfolioResponse>>());
        }
    }
}