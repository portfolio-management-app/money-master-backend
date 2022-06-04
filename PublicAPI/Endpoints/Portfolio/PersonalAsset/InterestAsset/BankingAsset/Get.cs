using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{

    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, BankingAssetResponse>
    {
        private readonly IBankSavingService _bankSavingService;


        public Get(IBankSavingService bankSavingService)
        {
            _bankSavingService = bankSavingService;

        }

        [HttpGet("bankSaving/{assetId}")]
        public override async Task<ActionResult<BankingAssetResponse>> HandleAsync([FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundAsset = _bankSavingService.GetById(request.AssetId);
            if (foundAsset is null)
                return NotFound();

            return Ok(foundAsset.Adapt<BankingAssetResponse>());
        }
    }

}