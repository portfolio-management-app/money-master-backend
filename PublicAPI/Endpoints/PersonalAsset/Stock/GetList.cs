using System.Collections.Generic;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.Stock
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public class GetList: EndpointBaseSync.WithRequest<int>.WithActionResult<List<StockResponse>>
    {
        private readonly IStockService _stockService;

        public GetList(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("stock")]
        public override ActionResult<List<StockResponse>> Handle(int portfolioId)
        {
            var stockList = _stockService.GetListStockByPortfolio(portfolioId);
            return Ok(stockList.Adapt<List<StockResponse>>()); 
        }
    }
}