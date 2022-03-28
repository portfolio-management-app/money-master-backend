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
    public class Create : BaseRealEstateEndpoint<CreateNewRealEstateAssetRequest, RealEstateResponse>
    {
        [HttpPost("realEstate")]
        public override ActionResult<RealEstateResponse> Handle(
            [FromMultipleSource] CreateNewRealEstateAssetRequest request)
        {
            var dto = request.CreateNewRealEstateAssetCommand.Adapt<RealEstateDto>();
            var newRealEstate = RealEstateService.CreateNewRealEstateAsset(request.PortfolioId, dto);

            return Ok(newRealEstate.Adapt<RealEstateResponse>());
        }

        public Create(IRealEstateService realEstateService) : base(realEstateService)
        {
        }
    }
}