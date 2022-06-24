using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    public class GetSumOfPortfolio: BasePortfolioRelatedEndpoint<int, decimal>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IReportService _reportService;

        public GetSumOfPortfolio(IAuthorizationService authorizationService, IReportService reportService)
        {
            _authorizationService = authorizationService;
            _reportService = reportService;
        }
        
        [HttpGet("sum")]
        public override async Task<ActionResult<decimal>> HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
            {
                return Unauthorized(NotAllowedPortfolioMessage);
            }

            try
            {
                var sum = await _reportService.GetSumValueOfPortfolio(portfolioId);
                return sum;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}