using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Report
{
    public class GetPieChart : BasePortfolioRelatedEndpoint<int, List<PieChartResponse>>
    {
        private readonly IReportService _reportService;
        private readonly IAuthorizationService _authorizationService;

        public GetPieChart(IReportService reportService, IAuthorizationService authorizationService)
        {
            _reportService = reportService;
            _authorizationService = authorizationService;
        }

        [HttpGet("pieChart")]
        public override async Task<ActionResult<List<PieChartResponse>>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return Unauthorized($"You are not allowed to this portfolio: {portfolioId}");

            var result = await _reportService.GetPieChart(portfolioId);

            return Ok(result);
        }
    }
}