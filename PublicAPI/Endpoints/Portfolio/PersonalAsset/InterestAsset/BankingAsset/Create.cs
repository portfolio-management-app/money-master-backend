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
    [Route("portfolio/{portfolioId}")]
    public class Create : EndpointBaseSync.WithRequest<CreateNewBankingAssetRequest>.WithActionResult<
        BankingAssetResponse>
    {
        private readonly IInterestAssetService _interestAssetService;

        public Create(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }

        [HttpPost("bankSaving")]
        public override ActionResult<BankingAssetResponse> Handle(
            [FromMultipleSource] CreateNewBankingAssetRequest request)
        {
            var userId = (int)HttpContext.Items["userId"]!;
            var dto = request.CreateNewBankingAssetCommand.Adapt<CreateNewBankSavingAssetDto>();
            var newBankSavingAsset = _interestAssetService.AddBankSavingAsset(request.PortfolioId, dto);

            return newBankSavingAsset.Adapt<BankingAssetResponse>();
        }
    }
}