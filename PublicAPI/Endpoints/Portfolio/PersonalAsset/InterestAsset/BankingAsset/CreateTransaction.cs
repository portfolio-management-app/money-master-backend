using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using Mapster;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class CreateTransaction: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly IBankSavingService _bankSavingService;
        private readonly IAssetTransactionService _transactionService;

        public CreateTransaction(IBankSavingService bankSavingService, IAssetTransactionService transactionService)
        {
            _bankSavingService = bankSavingService;
            _transactionService = transactionService;
        }
        
        [HttpPost("bankSaving/{assetId}/transaction")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundBank = _bankSavingService.GetById(request.AssetId);
            var command = request.CreateTransactionCommand;
            if (command.DestinationAssetId == null) return BadRequest("Destination");
            try
            {
                var transaction = command.TransactionType switch
                {
                    "withdrawValue" => await _transactionService
                        .CreateWithdrawToCashTransaction(foundBank, command.DestinationAssetId.Value, command.Amount,
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