using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.Report
{
    public class GetSankeyChartRequest
    {
        [FromQuery] public DateTime? StartTime { get; set; }
        [FromQuery] public DateTime? EndTime { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}