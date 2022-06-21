using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class GetInvestFundTransactionRequest
    {
        [FromRoute] public int PortfolioId { get; set; }
        [FromQuery] public int? PageNumber { get; set; } = null;
        [FromQuery] public int? PageSize { get; set; } = null;
        [FromQuery] public DateTime? StartDate { get; set; } = null;
        [FromQuery] public DateTime? EndDate { get; set; } = null;

        [FromQuery] public string Type { get; set; } = "all";
    }
}