using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.InvestFundAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class GetInvestFund : BasePortfolioRelatedEndpoint<int, InvestFundResponse>
    {
        private readonly IInvestFundService _investFundService;

        public GetInvestFund(IInvestFundService investFundService)
        {
            _investFundService = investFundService;
        }

        [HttpGet("fund")]
        public override async Task<ActionResult<InvestFundResponse>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            var investFund = _investFundService.GetInvestFundByPortfolio(portfolioId);
            if (investFund is null)
                return NotFound();
            var response = investFund.Adapt<InvestFundResponse>();
            response.InitialCurrency = investFund.Portfolio.InitialCurrency;
            return Ok(response);
        }
    }
}