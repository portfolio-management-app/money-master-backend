using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock{

    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, StockResponse>
    {
        private readonly IStockService _stockService;


        public Get(IStockService stockService)
        {
            _stockService = stockService;

        }

        [HttpGet("stock/{assetId}")]
        public override async Task<ActionResult<StockResponse>> HandleAsync([FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundAsset = _stockService.GetById(request.AssetId);
            if (foundAsset is null)
                return NotFound();

            return Ok(foundAsset.Adapt<StockResponse>());
        }
    }

}