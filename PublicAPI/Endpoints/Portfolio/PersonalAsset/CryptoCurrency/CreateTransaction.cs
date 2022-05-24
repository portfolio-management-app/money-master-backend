using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class CreateTransaction: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly ICryptoService _cryptoService;
        private readonly IAssetTransactionService _transactionService;

        public CreateTransaction(ICryptoService cryptoService, IAssetTransactionService transactionService)
        {
            _cryptoService = cryptoService;
            _transactionService = transactionService;
        }
        
        [HttpPost("crypto/{assetId}/transaction")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {

            var foundCrypto = _cryptoService.GetById(request.AssetId);
            var command = request.CreateTransactionCommand;
            if (command.DestinationAssetId == null) return BadRequest("Destination");
            try
            {
                var transaction = command.TransactionType switch
                {
                    "withdrawValue" => await _transactionService
                        .CreateWithdrawToCashTransaction(foundCrypto, command.DestinationAssetId.Value, command.Amount,
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