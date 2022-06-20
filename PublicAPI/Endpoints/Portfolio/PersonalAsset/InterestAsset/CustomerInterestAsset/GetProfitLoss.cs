using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.ReportAggregate;
using ApplicationCore.ReportAggregate.Models;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class GetProfitLoss : BasePortfolioRelatedEndpoint<GetProfitLossRequest, List<ProfitLossBasis>>
    {
        private readonly IReportService _reportService;

        public GetProfitLoss(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("custom/{assetId}/profitLoss")]
        public override async Task<ActionResult<List<ProfitLossBasis>>> HandleAsync(
            [FromMultipleSource] GetProfitLossRequest request, CancellationToken cancellationToken = new())
        {
            string assetType = "custom";
            try
            {
                var listProfitLosses = request.Period switch
                {
                    "day" => await _reportService.GetPeriodProfitLossByAsset(request.AssetId, assetType),
                    "week" => await _reportService.GetPeriodProfitLossByAsset(request.AssetId, assetType, "week"),
                    "month" => await _reportService.GetPeriodProfitLossByAsset(request.AssetId, assetType, "month"),
                    _ => throw new InvalidOperationException("Unsupported type ")
                };
                return Ok(listProfitLosses);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }   
}