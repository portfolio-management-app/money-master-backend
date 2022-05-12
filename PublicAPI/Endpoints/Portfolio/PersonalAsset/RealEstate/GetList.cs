using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class GetList : BasePortfolioRelatedEndpoint<int, List<RealEstateResponse>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRealEstateService _realEstateService;
        
        
        [HttpGet("realEstate")]
        public override async Task<ActionResult<List<RealEstateResponse>>> HandleAsync
            (int portfolioId, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return Unauthorized($"You are not allowed to this portfolio: {portfolioId}");
            var list = await _realEstateService.ListByPortfolio(portfolioId);
            return Ok(list.Adapt<List<RealEstateResponse>>());
        }

        public GetList(IRealEstateService realEstateService, IAuthorizationService authorizationService)
        {
            _realEstateService = realEstateService;
            _authorizationService = authorizationService;
        }
    }
}