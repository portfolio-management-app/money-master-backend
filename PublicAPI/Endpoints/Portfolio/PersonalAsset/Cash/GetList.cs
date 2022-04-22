using System.Collections.Generic;
using ApplicationCore.AssetAggregate.CashAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public class GetList : EndpointBaseSync.WithRequest<int>.WithActionResult<List<CashResponse>>
    {
        private readonly ICashService _cashService;

        public GetList(ICashService cashService)
        {
            _cashService = cashService;
        }

        [HttpGet("cash")]
        public override ActionResult<List<CashResponse>> Handle(int portfolioId)
        {
            return Ok(_cashService.ListByPortfolio(portfolioId).Adapt<List<CashResponse>>());
        }
    }
}