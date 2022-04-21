using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    public class GetList: EndpointBaseAsync.WithRequest<int>.WithActionResult<List<StockResponse>>
    {
        private readonly IStockService _stockService;

        public GetList(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("stock")]
        public override async Task<ActionResult<List<StockResponse>>> HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
             var stockList =  _stockService.ListByPortfolio(portfolioId);
             return Ok(stockList.Adapt<List<StockResponse>>()); 
        }
    }
}