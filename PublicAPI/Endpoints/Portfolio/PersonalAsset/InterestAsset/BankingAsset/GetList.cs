using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class GetList : BasePortfolioRelatedEndpoint<int, GetListBankSavingAssetResponse>
    {
        private readonly IInterestAssetService _interestAssetService;
        private readonly IAuthorizationService _authorizationService;

        public GetList(IInterestAssetService interestAssetService, IAuthorizationService authorizationService)
        {
            _interestAssetService = interestAssetService;
            _authorizationService = authorizationService;
        }

        [HttpGet("bankSaving")]
        public override async Task<ActionResult<GetListBankSavingAssetResponse>> HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var list = _interestAssetService.GetAllPortfolioBankSavingAssets(portfolioId);
            return Ok(list.Adapt<List<GetListBankSavingAssetResponse>>());
        }
    }
}