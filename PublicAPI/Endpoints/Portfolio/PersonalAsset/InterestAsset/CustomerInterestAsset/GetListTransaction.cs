using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Endpoints.Portfolio.Transactions;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class GetListTransaction : BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly ICustomAssetService _customAssetService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(ICustomAssetService customAssetService, IAssetTransactionService transactionService)
        {
            _customAssetService = customAssetService;
            _transactionService = transactionService;
        }

        [HttpGet("custom/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromMultipleSource] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var customAsset = _customAssetService.GetById(request.AssetId);
            if (customAsset is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(customAsset, request.PageNumber,
                request.PageSize, request.StartDate, request.EndDate, request.Type);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }
    }
}