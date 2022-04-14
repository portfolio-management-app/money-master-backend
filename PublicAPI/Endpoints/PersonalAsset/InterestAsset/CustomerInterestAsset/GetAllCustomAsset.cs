using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    [Authorize]
    [Route("portfolio/{portfolioId:int}")]
    public class GetAllCustomAsset: EndpointBaseAsync.WithRequest<int>.WithActionResult<List<GetAllCustomAssetResponse>>
    {
        private readonly IInterestAssetService _interestAssetService;

        public GetAllCustomAsset(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }

        [HttpGet("custom")]
        public override async Task<ActionResult<List<GetAllCustomAssetResponse>>> HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            var listAssets = _interestAssetService.GetALlCustomInterestAssets(portfolioId);

            var groups = listAssets
                .GroupBy(asset
                        => new {asset.CustomInterestAssetInfo.Id,asset.CustomInterestAssetInfo.Name},
                    (key,g)
                        => new {CategoryId = key.Id, CategoryName = key.Name, Assets = g.ToList()} )
                .ToList();

            return await Task.FromResult(Ok(groups.Adapt<List<GetAllCustomAssetResponse>>())); 
        }
    }
}