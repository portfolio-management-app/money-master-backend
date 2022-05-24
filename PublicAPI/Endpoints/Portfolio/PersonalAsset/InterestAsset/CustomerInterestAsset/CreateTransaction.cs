using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class CreateTransaction: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly ICustomAssetService _customAssetService;
        private readonly IAssetTransactionService _transactionService;

        public CreateTransaction(ICustomAssetService customAssetService, IAssetTransactionService transactionService)
        {
            _customAssetService = customAssetService;
            _transactionService = transactionService;
        }
        
        [HttpPost("custom/{assetId}/transaction")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var found = _customAssetService.GetById(request.AssetId);
            var command = request.CreateTransactionCommand;
            if (command.DestinationAssetId == null) return BadRequest("Destination");
            try
            {
                var transaction = command.TransactionType switch
                {
                    "withdrawValue" => await _transactionService
                        .CreateWithdrawToCashTransaction(found, command.DestinationAssetId.Value, command.Amount,
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