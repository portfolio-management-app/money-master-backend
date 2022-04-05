using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    public class GetList : BaseRealEstateEndpoint<int, List<RealEstateResponse>>
    {
        [HttpGet("realEstate")]
        public override async Task<ActionResult<List<RealEstateResponse>>> HandleAsync
            (int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(portfolioId))
            {
                return Unauthorized($"You are not allowed to this portfolio: {portfolioId}"); 
            }
            var list = RealEstateService.GetAllRealEstateAssetByPortfolio(portfolioId);
            return Ok(list.Adapt<List<RealEstateResponse>>());
        }
        public GetList(IRealEstateService realEstateService, IAuthorizationService authorizationService) 
            : base(realEstateService,authorizationService )
        {
        }
    }
}