using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    public class Edit : BaseRealEstateEndpoint<EditRealEstateAssetRequest, RealEstateResponse>
    {
        [HttpPut("realEstate/{realEstateId}")]
        public override ActionResult<RealEstateResponse> Handle
            ([FromMultipleSource] EditRealEstateAssetRequest request)
        {
            var dto = request.EditRealEstateAssetCommand.Adapt<RealEstateDto>();
            var result = RealEstateService.UpdateRealEstateAsset(request.PortfolioId, request.RealEstateId, dto);
            if (result is null)
                return NotFound("Could not find your asset");
            return Ok(result.Adapt<RealEstateResponse>());
        }

        public Edit(IRealEstateService realEstateService) : base(realEstateService)
        {
        }
    }
}