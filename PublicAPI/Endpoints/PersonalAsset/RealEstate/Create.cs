using System.Collections.Generic;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    public class Create: EndpointBaseSync.WithRequest<CreateNewRealEstateAssetRequest>.WithActionResult<List<object>>
    {
        private readonly IRealEstateService _realEstateService;

        public Create(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        [HttpPost("realEstate")]
        public override ActionResult<List<object>> Handle([FromMultipleSource]CreateNewRealEstateAssetRequest request)
        {
            var dto = request.CreateNewRealEstateAssetCommand.Adapt<CreateNewRealEstateDto>();
            var newRealEstate = _realEstateService.CreateNewRealEstateAsset(request.PortfolioId, dto);

            return Ok(newRealEstate.Adapt<RealEstateResponse>()); 
        }
    }
}