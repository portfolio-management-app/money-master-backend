using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset
{
    public class GetListTransactionRequest
    {
        [FromRoute] public int AssetId { get; set; }
        [FromRoute] public int PortfolioId { get; set; }

        [FromQuery] public int? PageNumber { get; set; } = null;
        [FromQuery] public int? PageSize { get; set; } = null;
        [FromQuery] public DateTime? StartDate { get; set; } = null;
        [FromQuery] public DateTime? EndDate { get; set; } = null;
    }
}