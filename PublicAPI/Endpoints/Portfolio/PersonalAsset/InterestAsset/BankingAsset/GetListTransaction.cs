using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class GetListTransaction : BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly IBankSavingService _bankService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(IBankSavingService bankService, IAssetTransactionService transactionService)
        {
            _bankService = bankService;
            _transactionService = transactionService;
        }

        [HttpGet("bankSaving/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromRoute] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var bankSavingAsset = _bankService.GetById(request.AssetId);
            if (bankSavingAsset is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(bankSavingAsset);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }
    }

    
}