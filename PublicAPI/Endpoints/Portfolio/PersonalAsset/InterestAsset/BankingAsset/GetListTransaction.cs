using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class GetListTransaction : BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<object>>
    {
        private readonly IInterestAssetService _bankService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(IInterestAssetService bankService, IAssetTransactionService transactionService)
        {
            _bankService = bankService;
            _transactionService = transactionService;
        }

        [HttpGet("bankSaving/{assetId}/transactions")]
        public override async Task<ActionResult<List<object>>> HandleAsync(
            [FromRoute] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var bankSavingAsset = _bankService.GetBankSavingAssetById(request.AssetId);
            if (bankSavingAsset is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(bankSavingAsset);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }
    }

    
}