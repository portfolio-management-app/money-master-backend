using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class Delete: BasePortfolioRelatedEndpoint<PortfolioAssetRequest, CashResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICashService _cashService; 
        public Delete(IAuthorizationService authorizationService, ICashService cashService)
        {
            _authorizationService = authorizationService;
            _cashService = cashService;
        }

        [HttpDelete("cash/{assetId}")]
        public override async Task<ActionResult<CashResponse>> HandleAsync([FromMultipleSource]PortfolioAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var cash = _cashService.SetAssetToDelete(request.AssetId);
            return Ok(cash.Adapt<CashResponse>());
        }
    }
}