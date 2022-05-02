using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class CreateTransaction: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly IStockService _stockService;
        private readonly IAssetTransactionService _transactionService;

        public CreateTransaction(IStockService stockService, IAssetTransactionService transactionService)
        {
            _stockService = stockService;
            _transactionService = transactionService;
        }
        
        [HttpPost("stock/{assetId}/transaction")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var found = _stockService.GetById(request.AssetId);
            var command = request.CreateTransactionCommand;
            if (command.DestinationAssetId == null) return BadRequest("Destination");
            try
            {
                var transaction = command.TransactionType switch
                {
                    "withdrawValue" => await _transactionService
                        .WithdrawToCash(found, command.DestinationAssetId.Value, command.Amount,
                            command.CurrencyCode, command.IsTransferringAll),
                    _ => await _transactionService.Fake()
                };
                return Ok(new TransactionResponse(transaction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}