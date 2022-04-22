using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Report
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public class GetPieChart : EndpointBaseAsync.WithRequest<int>.WithActionResult<List<PieChartResponse>>
    {
        private readonly IReportService _reportService;

        public GetPieChart(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("pieChart")]
        public override async Task<ActionResult<List<PieChartResponse>>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            var result = await _reportService.GetPieChart(portfolioId);

            return Ok(result);
        }
    }
}