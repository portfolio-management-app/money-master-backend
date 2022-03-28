using System.Threading;
using System.Threading.Tasks;
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
   

        public override async Task<ActionResult<RealEstateResponse>> HandleAsync([FromMultipleSource]EditRealEstateAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {   
            if (!await IsAllowedToExecute(request.PortfolioId))
            {
                return Unauthorized($"You are not allowed to this portfolio: {request.PortfolioId}"); 
            }
            var dto = request.EditRealEstateAssetCommand.Adapt<RealEstateDto>();
            var result = RealEstateService.UpdateRealEstateAsset(request.PortfolioId, request.RealEstateId, dto);
            if (result is null)
                return NotFound("Could not find your asset");
            return Ok(result.Adapt<RealEstateResponse>());
        }    
        public Edit(IRealEstateService realEstateService, IAuthorizationService authorizationService) : base(realEstateService, authorizationService)
        {
        }
    }
}