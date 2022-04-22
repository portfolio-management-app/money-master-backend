using System.Collections.Generic;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    [Authorize]
    [Route("portfolio/{portfolioId}/bankSaving")]
    public class GetList : EndpointBaseSync.WithRequest<int>.WithActionResult<List<GetListBankSavingAssetResponse>>
    {
        private readonly IInterestAssetService _interestAssetService;

        public GetList(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }

        [HttpGet]
        public override ActionResult<List<GetListBankSavingAssetResponse>> Handle(int portfolioId)
        {
            var list = _interestAssetService.GetAllPortfolioBankSavingAssets(portfolioId);
            return Ok(list.Adapt<List<GetListBankSavingAssetResponse>>());
        }
    }
}