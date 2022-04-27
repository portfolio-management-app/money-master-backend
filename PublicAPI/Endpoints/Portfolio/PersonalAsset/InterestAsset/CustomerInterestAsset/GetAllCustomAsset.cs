using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class
        GetAllCustomAsset : BasePortfolioRelatedEndpoint<int,List<GetAllCustomAssetResponse>>
    {
        private readonly ICustomAssetService _customAssetService;
        private readonly IAuthorizationService _authorizationService;

        public GetAllCustomAsset(ICustomAssetService customAssetService, IAuthorizationService authorizationService)
        {
            _customAssetService = customAssetService;
            _authorizationService = authorizationService;
        }

        [HttpGet("custom")]
        public override async Task<ActionResult<List<GetAllCustomAssetResponse>>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var listAssets = _customAssetService.GetAllCustomInterestAssetsByPortfolio(portfolioId);

            var groups = listAssets
                .GroupBy(asset
                        => new { asset.CustomInterestAssetInfo.Id, asset.CustomInterestAssetInfo.Name },
                    (key, g)
                        => new { CategoryId = key.Id, CategoryName = key.Name, Assets = g.ToList() })
                .ToList();

            return await Task.FromResult(Ok(groups.Adapt<List<GetAllCustomAssetResponse>>()));
        }
    }
}