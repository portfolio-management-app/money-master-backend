using System.Collections.Generic;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    [Authorize]
    [Route("portfolio/{portfolioId}/realEstate")]
    public class GetList: EndpointBaseSync.WithRequest<int>.WithActionResult<List<RealEstateResponse>>
    {
        private readonly IRealEstateService _realEstateService;

        public GetList(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }
        
        [HttpGet]
        public override ActionResult<List<RealEstateResponse>> Handle(int portfolioId)
        {
            var list = _realEstateService.GetAllRealEstateAssetByPortfolio(portfolioId);
            return Ok(list.Adapt<List<RealEstateResponse>>()); 
        }
    }
}