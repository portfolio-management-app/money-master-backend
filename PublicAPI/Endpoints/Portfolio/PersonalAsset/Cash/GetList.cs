using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{

    public class GetList : BasePortfolioRelatedEndpoint<int,List<CashResponse>>
    {
        private readonly ICashService _cashService;
        private readonly IAuthorizationService _authorizationService;

        public GetList(ICashService cashService, IAuthorizationService authorizationService)
        {
            _cashService = cashService;
            _authorizationService = authorizationService;
        }

        [HttpGet("cash")]
        public override async Task<ActionResult<List<CashResponse>>> HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                            return  Unauthorized(NotAllowedPortfolioMessage);
            var result = await _cashService.ListByPortfolio(portfolioId); 
            return Ok(result.Adapt<List<CashResponse>>());
        }
    }
}