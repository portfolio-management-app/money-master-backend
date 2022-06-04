using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.InvestFundAggregate;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using ApplicationCore.Entity.Asset;
using ApplicationCore.PortfolioAggregate;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.TransactionAggregate;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class AddToInvestFund : BasePortfolioRelatedEndpoint<AddToInvestFundRequest, object>
    {
        private readonly IInvestFundService _investFundService;
        private readonly IPortfolioService _portfolioService;

        private readonly IAssetTransactionService _assetTransactionService;
        private readonly IAuthorizationService _authorizationService;

        public AddToInvestFund(IInvestFundService investFundService, IPortfolioService portfolioService, IAuthorizationService authorizationService, IAssetTransactionService assetTransactionService)
        {
            _investFundService = investFundService;
            _portfolioService = portfolioService;
            _authorizationService = authorizationService;
            _assetTransactionService = assetTransactionService;
        }

        [HttpPost("fund")]
        public override async Task<ActionResult<object>> HandleAsync
            ([FromMultipleSource] AddToInvestFundRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized("You are allowed for this portfolio");

            var personalAsset = _portfolioService.GetAssetByPortfolioAndAssetId(request.PortfolioId,
                request.AddToInvestFundCommand.ReferentialAssetType,
                request.AddToInvestFundCommand.ReferentialAssetId);
            try
            {
                var result = await _investFundService
                    .AddToInvestFund(request.PortfolioId, personalAsset, request.AddToInvestFundCommand.Amount,
                        request.AddToInvestFundCommand.CurrencyCode, request.AddToInvestFundCommand.IsTransferringAll);
                await _assetTransactionService.CreateMoveToFundTransaction(request.PortfolioId, personalAsset, request.AddToInvestFundCommand.Amount,
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