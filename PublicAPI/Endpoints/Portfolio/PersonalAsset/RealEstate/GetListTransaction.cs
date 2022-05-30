using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Endpoints.Portfolio.Transactions;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class GetListTransaction: BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly IRealEstateService _realEstateService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(IRealEstateService realEstateService, IAssetTransactionService transactionService)
        {
            _realEstateService = realEstateService;
            _transactionService = transactionService;
        }

        [HttpGet("realEstate/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromRoute] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var realEstateAsset = _realEstateService.GetById(request.AssetId);
            if (realEstateAsset is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(realEstateAsset);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        } 
    }
}