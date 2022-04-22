using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    [Authorize]
    [Route("portfolio/{portfolioId}/bankSaving/{bankSavingId}")]
    public class Edit : EndpointBaseSync.WithRequest<EditBankingAssetRequest>.WithActionResult<BankingAssetResponse>
    {
        private IInterestAssetService _interestAssetService;

        public Edit(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }

        [HttpPut]
        public override ActionResult<BankingAssetResponse> Handle(
            [FromMultipleSource] EditBankingAssetRequest request)
        {
            var dto = request.EditBankAssetCommand.Adapt<EditBankSavingAssetDto>();
            var result = _interestAssetService.EditBankSavingAsset(request.PortfolioId, request.BankSavingId, dto);

            if (result is null)
                return NotFound("Could not find the asset");
            return Ok(result.Adapt<BankingAssetResponse>());
        }
    }
}