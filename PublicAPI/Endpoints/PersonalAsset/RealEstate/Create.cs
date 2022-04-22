using System.Collections.Generic;
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
    public class Create : BaseRealEstateEndpoint<CreateNewRealEstateAssetRequest, RealEstateResponse>
    {
        public Create(IRealEstateService realEstateService, IAuthorizationService authorizationService) : base(
            realEstateService, authorizationService)
        {
        }


        [HttpPost("realEstate")]
        public override async Task<ActionResult<RealEstateResponse>> HandleAsync(
            [FromMultipleSource] CreateNewRealEstateAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId))
                return Unauthorized($"You are not allowed to this portfolio: {request.PortfolioId}");
            var dto = request.CreateNewRealEstateAssetCommand.Adapt<RealEstateDto>();
            var newRealEstate = RealEstateService.CreateNewRealEstateAsset(request.PortfolioId, dto);

            return Ok(newRealEstate.Adapt<RealEstateResponse>());
        }
    }
}