using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using ApplicationCore.ReportAggregate.Models;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class GetProfitLoss : BasePortfolioRelatedEndpoint<GetProfitLossRequest, List<ProfitLossBasis>>
    {
        private readonly IReportService _reportService;

        public GetProfitLoss(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("crypto/{assetId}/profitLoss")]
        public override async Task<ActionResult<List<ProfitLossBasis>>> HandleAsync(
            [FromMultipleSource] GetProfitLossRequest request, CancellationToken cancellationToken = new())
        {
            var listProfitLosses = request.Period switch
            {
                "day" => await _reportService.GetPeriodProfitLossByAsset(request.AssetId, "crypto"),
                "week" => throw new NotImplementedException(),
                "month" => throw new NotImplementedException(),
                _ => throw new InvalidOperationException("Unsupported type ")
            };

            return Ok(listProfitLosses);
        }
    }
}