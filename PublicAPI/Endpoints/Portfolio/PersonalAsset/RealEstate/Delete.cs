using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class Delete : BasePortfolioRelatedEndpoint<PortfolioAssetRequest, RealEstateResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRealEstateService _realEstateService;

        public Delete(IRealEstateService realEstateService, IAuthorizationService authorizationService)
        {
            _realEstateService = realEstateService;
            _authorizationService = authorizationService;
        }

        [HttpDelete("realEstate/{assetId}")]
        public override async Task<ActionResult<RealEstateResponse>> HandleAsync(
            [FromMultipleSource] PortfolioAssetRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage); 
            var realEstateAsset = _realEstateService.SetAssetToDelete(request.AssetId);
            return Ok(realEstateAsset.Adapt<RealEstateResponse>());
        }
    }
}