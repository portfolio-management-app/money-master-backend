using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class Create : BasePortfolioRelatedEndpoint<CreateNewStockRequest, StockResponse>
    {
        private readonly IStockService _stockService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssetTransactionService _transactionService;

        public Create(IStockService stockService, IAuthorizationService authorizationService,
            IAssetTransactionService transactionService)
        {
            _stockService = stockService;
            _authorizationService = authorizationService;
            _transactionService = transactionService;
        }

        [HttpPost("stock")]
        public override async Task<ActionResult<StockResponse>> HandleAsync(
            [FromMultipleSource] CreateNewStockRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.CreateNewStockCommand.Adapt<StockDto>();
            try
            {
                var newStock = await _stockService.CreateNewStockAsset(request.PortfolioId, dto);
                _ = _transactionService.AddCreateNewAssetTransaction(request.PortfolioId,newStock,
                    newStock.PurchasePrice * newStock.CurrentAmountHolding, newStock.CurrencyCode
                    , dto.IsUsingInvestFund, dto.IsUsingCash, dto.UsingCashId
                    , dto.Fee, dto.Tax);
                return Ok(newStock.Adapt<StockResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}