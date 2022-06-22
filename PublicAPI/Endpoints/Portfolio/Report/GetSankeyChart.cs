using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.Report
{
    public class GetSankeyChart : BasePortfolioRelatedEndpoint<GetSankeyChartRequest, List<SankeyChartResponse>>
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
            ([FromMultipleSource]GetSankeyChartRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized($"You are not allowed to this portfolio: {request.PortfolioId}");
            var listSankeyBasis = await _reportService.GetSankeyChart(request.PortfolioId,request.StartTime, request.EndTime);
            return Ok(listSankeyBasis.Select(basis => basis.Adapt<SankeyChartResponse>()));
        }
    }
}