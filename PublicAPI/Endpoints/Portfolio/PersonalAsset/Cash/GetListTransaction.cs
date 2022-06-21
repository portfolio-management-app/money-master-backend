using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.Portfolio.Transactions;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class GetListTransaction : BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly ICashService _cashService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(ICashService cashService, IAssetTransactionService transactionService)
        {
            _cashService = cashService;
            _transactionService = transactionService;
        }

        [HttpGet("cash/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromMultipleSource] GetListTransactionRequest request, CancellationToken cancellationToken = new())
        {
            var foundCash = _cashService.GetById(request.AssetId);
            if (foundCash is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(foundCash, request.PageNumber,
                request.PageSize, request.StartDate, request.EndDate);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }
    }
}