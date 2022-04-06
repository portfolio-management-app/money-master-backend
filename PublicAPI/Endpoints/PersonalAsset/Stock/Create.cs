using System.Xml;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.Stock
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public class Create: EndpointBaseSync.WithRequest<CreateNewStockRequest>.WithActionResult<StockResponse>
    {

        private readonly IStockService _stockService;

        public Create(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("stock")]
        public override ActionResult<StockResponse> Handle([FromMultipleSource] CreateNewStockRequest request)
        {
            var dto = request.CreateNewStockCommand.Adapt<StockDto>();
            var newStock = _stockService.CreateNewStockAsset(request.PortfolioId, dto); 
            return Ok(newStock.Adapt<StockResponse>()); 
        }
    }
}