using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class Edit : BasePortfolioRelatedEndpoint<EditRealEstateAssetRequest, RealEstateResponse>
    {

        private readonly IAuthorizationService _authorizationService;
        private readonly IRealEstateService _realEstateService;

        public Edit(IAuthorizationService authorizationService, IRealEstateService realEstateService)
        {
            _authorizationService = authorizationService;
            _realEstateService = realEstateService;
        }

        [HttpPut("realEstate/{realEstateId}")]
        public override async Task<ActionResult<RealEstateResponse>> HandleAsync(
            [FromMultipleSource] EditRealEstateAssetRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.EditRealEstateAssetCommand.Adapt<RealEstateDto>();
            var result = _realEstateService.UpdateRealEstateAsset(request.PortfolioId, request.RealEstateId, dto);
            if (result is null)
                return NotFound("Could not find your asset");
            return Ok(result.Adapt<RealEstateResponse>());
        }


    }
}