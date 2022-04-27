using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class Delete: BasePortfolioRelatedEndpoint<PortfolioResourceRequest,StockResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IStockService _stockService;

        public Delete(IAuthorizationService authorizationService, IStockService stockService)
        {
            _authorizationService = authorizationService;
            _stockService = stockService;
        }
        
        [HttpDelete("stock/{assetId}")]
        public override async Task<ActionResult<StockResponse>> HandleAsync([FromMultipleSource]PortfolioResourceRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage); 
            var realEstateAsset = _stockService.SetAssetToDelete(request.AssetId);
            return Ok(realEstateAsset.Adapt<StockResponse>());
        }
    }
}