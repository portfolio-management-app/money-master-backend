using System.Collections.Generic;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    [Route("portfolio/{portfolioId}/realEstate")]
    public class GetList : BaseRealEstateEndpoint<int, List<RealEstateResponse>>
    {

        [HttpGet]
        public override ActionResult<List<RealEstateResponse>> Handle(int portfolioId)
        {
            var list = RealEstateService.GetAllRealEstateAssetByPortfolio(portfolioId);
            return Ok(list.Adapt<List<RealEstateResponse>>());
        }

        public GetList(IRealEstateService realEstateService) : base(realEstateService)
        {
        }
    }
}