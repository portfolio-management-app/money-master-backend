using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Report
{
    public class GetSankeyChart : BasePortfolioRelatedEndpoint<int, List<SankeyChartResponse>>
    {
        private readonly IReportService _reportService;
        private readonly IAuthorizationService _authorizationService;

        public GetSankeyChart(IReportService reportService, IAuthorizationService authorizationService)
        {
            _reportService = reportService;
            _authorizationService = authorizationService;
        }

        [HttpGet("sankey")]
        public override async Task<ActionResult<List<SankeyChartResponse>>> HandleAsync
            ([FromRoute] int portfolioId, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return Unauthorized($"You are not allowed to this portfolio: {portfolioId}");
            var listSankeyBasis = await _reportService.GetSankeyChart(portfolioId);
            return Ok(listSankeyBasis.Select(basis => basis.Adapt<SankeyChartResponse>()));
        }
    }
}