using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class Delete : BasePortfolioRelatedEndpoint<PortfolioAssetRequest, BankingAssetResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IBankSavingService _bankSavingService;

        public Delete(IAuthorizationService authorizationService, IBankSavingService bankSavingService)
        {
            _authorizationService = authorizationService;
            _bankSavingService = bankSavingService;
        }
        
        [HttpDelete("bankSaving/{assetId}")]
        public override async Task<ActionResult<BankingAssetResponse>> HandleAsync([FromMultipleSource]PortfolioAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var bankSavingAsset = _bankSavingService.SetAssetToDelete(request.AssetId);
            return Ok(bankSavingAsset.Adapt<BankingAssetResponse>());
        }
    }
}