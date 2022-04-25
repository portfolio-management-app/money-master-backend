using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.TransactionAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class GetListTransaction: BasePortfolioRelatedEndpoint<GetListTransactionRequest, List<TransactionResponse>>
    {
        private readonly IStockService _stockService;
        private readonly IAssetTransactionService _transactionService;

        public GetListTransaction(IStockService stockService, IAssetTransactionService transactionService)
        {
            _stockService = stockService;
            _transactionService = transactionService;
        }

        [HttpGet("stock/{assetId}/transactions")]
        public override async Task<ActionResult<List<TransactionResponse>>> HandleAsync(
            [FromRoute] GetListTransactionRequest request,
            CancellationToken cancellationToken = new())
        {
            var stock = _stockService.GetById(request.AssetId);
            if (stock is null)
                return NotFound();
            var listTransactions = _transactionService.GetTransactionListByAsset(stock);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }  
    }
}