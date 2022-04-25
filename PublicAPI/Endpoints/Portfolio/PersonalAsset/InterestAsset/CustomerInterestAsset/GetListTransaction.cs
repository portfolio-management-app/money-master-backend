using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class GetListTransaction: BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly IInterestAssetService _customAssetService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(IInterestAssetService customAssetService, IAssetTransactionService transactionService)
        {
            _customAssetService = customAssetService;
            _transactionService = transactionService;
        }

        [HttpGet("custom/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromRoute] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var customAsset = _customAssetService.GetCustomAssetById( request.AssetId);
            if (customAsset is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(customAsset);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        } 
    }
}