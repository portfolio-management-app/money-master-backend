using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.PortfolioAggregate;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio
{
    public class EditPortfolio: BasePortfolioRelatedEndpoint<EditPortfolioRequest, PortfolioResponse>
    {
        private readonly IPortfolioService _portfolioService;

        public EditPortfolio(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpPut]
        public override async Task<ActionResult<PortfolioResponse>> HandleAsync([FromMultipleSource]EditPortfolioRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var resultPortfolio = await _portfolioService.EditPortfolio(request.PortfolioId,
                request.EditPortfolioCommand.NewName, request.EditPortfolioCommand.NewCurrency);
            if (resultPortfolio is null)
                return NotFound();
            return Ok(resultPortfolio.Adapt<PortfolioResponse>()); 
        }
    }
}