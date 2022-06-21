using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.TransactionAggregate;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Endpoints.Portfolio.Transactions;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class GetListTransaction : BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly ICryptoService _cryptoService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(ICryptoService cryptoService, IAssetTransactionService transactionService)
        {
            _cryptoService = cryptoService;
            _transactionService = transactionService;
        }

        [HttpGet("crypto/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromMultipleSource] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var foundCrypto = _cryptoService.GetById(request.AssetId);
            if (foundCrypto is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(foundCrypto, request.PageNumber,
                request.PageSize, request.StartDate, request.EndDate);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }
    }
}