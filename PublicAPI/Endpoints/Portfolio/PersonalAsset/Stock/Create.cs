using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{

    public class Create : BasePortfolioRelatedEndpoint<CreateNewStockRequest, StockResponse>
    {
        private readonly IStockService _stockService;
        private readonly IAuthorizationService _authorizationService;

        public Create(IStockService stockService, IAuthorizationService authorizationService)
        {
            _stockService = stockService;
            _authorizationService = authorizationService;
        }

        [HttpPost("stock")]

        public override async Task<ActionResult<StockResponse>> HandleAsync(CreateNewStockRequest request, CancellationToken cancellationToken = new CancellationToken())
        {  
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.CreateNewStockCommand.Adapt<StockDto>(); 
            var newStock = _stockService.CreateNewStockAsset(request.PortfolioId, dto); 
            return Ok(newStock.Adapt<StockResponse>());
        }
    }
}