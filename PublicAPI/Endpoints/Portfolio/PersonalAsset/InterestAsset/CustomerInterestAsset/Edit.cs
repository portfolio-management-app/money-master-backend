using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.AssetAggregate.CustomAssetAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class Edit: BasePortfolioRelatedEndpoint<EditCustomAssetRequest,SingleCustomInterestAssetResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICustomAssetService _customAssetService;

        public Edit(IAuthorizationService authorizationService, ICustomAssetService customAssetService)
        {
            _authorizationService = authorizationService;
            _customAssetService = customAssetService;
        }
        
        [HttpPut("custom/{assetId}")]
        public override async Task<ActionResult<SingleCustomInterestAssetResponse>> HandleAsync([FromMultipleSource]EditCustomAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
            {
                return Unauthorized(NotAllowedPortfolioMessage); 
            }

            var dto = request.EditCustomAssetCommand.Adapt<EditCustomAssetDto>();
            var updated = _customAssetService.EditCustomAsset(request.AssetId, dto);
            if (updated is null)
                return NotFound();
            return updated.Adapt<SingleCustomInterestAssetResponse>();
        }
    }
}