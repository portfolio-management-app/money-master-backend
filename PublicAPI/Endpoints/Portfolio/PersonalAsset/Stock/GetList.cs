using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Stock
{
    public class GetList : BasePortfolioRelatedEndpoint<int, List<StockResponse>>
    {
        private readonly IStockService _stockService;
        private readonly IAuthorizationService _authorizationService;

        public GetList(IStockService stockService, IAuthorizationService authorizationService)
        {
            _stockService = stockService;
            _authorizationService = authorizationService;
        }

        [HttpGet("stock")]
        public override async Task<ActionResult<List<StockResponse>>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return Unauthorized($"You are not allowed to this portfolio: {portfolioId}");
            var stockList = _stockService.ListByPortfolio(portfolioId);
            return Ok(stockList.Adapt<List<StockResponse>>());
        }
    }
}