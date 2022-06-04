using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{

    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, SingleCustomInterestAssetResponse>
    {
        private readonly ICustomAssetService _customAssetService;


        public Get(ICustomAssetService customAssetService)
        {
            _customAssetService = customAssetService;

        }

        [HttpGet("custom/{assetId}")]
        public override async Task<ActionResult<SingleCustomInterestAssetResponse>> HandleAsync([FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundAsset = _customAssetService.GetById(request.AssetId);
            if (foundAsset is null)
                return NotFound();

            return Ok(foundAsset.Adapt<SingleCustomInterestAssetResponse>());
        }
    }

}