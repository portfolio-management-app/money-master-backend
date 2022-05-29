using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class CreateTransaction: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly IRealEstateService _realEstateService;
        private readonly IAssetTransactionService _transactionService;

        public CreateTransaction(IRealEstateService realEstateService, IAssetTransactionService transactionService)
        {
            _realEstateService = realEstateService;
            _transactionService = transactionService;
        }
        
        [HttpPost("realEstate/{assetId}/transaction")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var found = _realEstateService.GetById(request.AssetId);
            var command = request.CreateTransactionCommand;
            if (command.DestinationAssetId == null) return BadRequest("Destination");
            try
            {
                var transaction = command.TransactionType switch
                {
                    "withdrawValue" => await _transactionService
                        .CreateWithdrawToCashTransaction(found, command.DestinationAssetId.Value, command.Amount,
                            command.CurrencyCode, command.IsTransferringAll,command.Fee,command.Tax),
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