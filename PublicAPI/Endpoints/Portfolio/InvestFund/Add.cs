using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.InvestFundAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using ApplicationCore.Entity.Asset;
using ApplicationCore.PortfolioAggregate;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    [Route("portfolio/{portfolioId}/fund")]
    public class Add: EndpointBaseAsync.WithRequest<AddToInvestFundRequest>.WithActionResult<object>
    {
        private readonly IInvestFundService _investFundService;
        private readonly IPortfolioService _portfolioService;

        public Add(IInvestFundService investFundService, IPortfolioService portfolioService)
        {
            _investFundService = investFundService;
            _portfolioService = portfolioService;
        }
        
        [HttpPost]
        public override async Task<ActionResult<object>> HandleAsync
            ([FromMultipleSource]AddToInvestFundRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var personalAsset = _portfolioService.GetAssetByPortfolioAndAssetId(request.PortfolioId,
                    request.AddToInvestFundCommand.ReferentialAssetType,
                    request.AddToInvestFundCommand.ReferentialAssetId);
            try
            {
                var result = await _investFundService
                    .AddToInvestFund(request.PortfolioId, personalAsset, request.AddToInvestFundCommand.Amount,
                        request.AddToInvestFundCommand.CurrencyCode, request.AddToInvestFundCommand.IsTransferringAll);

                return Ok(result);
            }
            catch (OperationCanceledException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}