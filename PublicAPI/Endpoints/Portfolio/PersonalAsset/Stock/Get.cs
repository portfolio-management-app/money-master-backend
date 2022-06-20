using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Attributes;
using ApplicationCore;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, StockResponse>
    {
        private readonly IStockService _stockService;

        private readonly ExternalPriceFacade _priceFacade;


        public Get(IStockService stockService, ExternalPriceFacade priceFacade)
        {
            _stockService = stockService;
            _priceFacade = priceFacade;
        }

        [HttpGet("stock/{assetId}")]
        public override async Task<ActionResult<StockResponse>> HandleAsync(
            [FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new())
        {
            var foundAsset = _stockService.GetById(request.AssetId);
            if (foundAsset is null)
                return NotFound();
            var priceInUsdDto = await _priceFacade.StockPriceRepository.GetPrice(foundAsset.StockCode);
            foundAsset.CurrentPrice = priceInUsdDto.CurrentPrice;
            return Ok(foundAsset.Adapt<StockResponse>());
        }
    }
}