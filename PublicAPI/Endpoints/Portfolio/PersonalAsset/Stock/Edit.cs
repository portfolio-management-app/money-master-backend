using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class Edit: BasePortfolioRelatedEndpoint<EditStockRequest, StockResponse>
    {
        private readonly IStockService _stockService;
        private readonly IAuthorizationService _authorizationService;
        public Edit(IStockService stockService, IAuthorizationService authorizationService)
        {
            _stockService = stockService;
            _authorizationService = authorizationService;
        }

        [HttpPut("stock/{assetId}")]
        public override async Task<ActionResult<StockResponse>> HandleAsync
            ([FromMultipleSource]EditStockRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
            {
                return Unauthorized(NotAllowedPortfolioMessage); 
            }

            var dto = request.EditStockCommand.Adapt<EditStockDto>();
            var updatedStock =  _stockService.EditStock(request.AssetId, dto); 
            if(updatedStock is null)
                return NotFound(); 
            return Ok(updatedStock.Adapt<StockResponse>()); 
        }
    }
}