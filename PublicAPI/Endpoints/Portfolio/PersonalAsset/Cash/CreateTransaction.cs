using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class CreateTransaction: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly ICashService _cashService;
        private readonly IAssetTransactionService _transactionService;

        public CreateTransaction(ICashService cashService, IAssetTransactionService transactionService)
        {
            _cashService = cashService;
            _transactionService = transactionService;
        }
        
        [HttpPost("cash/{assetId}/transaction")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {

            var foundCash = _cashService.GetById(request.AssetId);
            var command = request.CreateTransactionCommand;
            if (command.DestinationAssetId == null) return BadRequest("Destination");
            try
            {
                var transaction = command.TransactionType switch
                {
                    "withdrawToCash" => await _transactionService
                        .CreateWithdrawToCashTransaction(foundCash, command.DestinationAssetId.Value, command.Amount,
                            command.CurrencyCode, command.IsTransferringAll, command.Fee, command.Tax),
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