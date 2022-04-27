using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.Entity.Asset;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class Delete : BasePortfolioRelatedEndpoint<PortfolioResourceRequest, SingleCustomInterestAssetResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICustomAssetService _customAssetService;

        public Delete(IAuthorizationService authorizationService, ICustomAssetService customAssetService)
        {
            _authorizationService = authorizationService;
            _customAssetService = customAssetService;
        }
        
        [HttpDelete("custom/{assetId}")]
        public override async Task<ActionResult<SingleCustomInterestAssetResponse>> HandleAsync(
            [FromMultipleSource]PortfolioResourceRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var custom = _customAssetService.SetAssetToDelete(request.AssetId);
            return Ok(custom.Adapt<SingleCustomInterestAssetResponse>());
        }
    }
}