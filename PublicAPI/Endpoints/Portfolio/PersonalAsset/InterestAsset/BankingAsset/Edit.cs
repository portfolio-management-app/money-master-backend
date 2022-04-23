using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class Edit : BasePortfolioRelatedEndpoint<EditBankingAssetRequest, BankingAssetResponse>
    {
        private IInterestAssetService _interestAssetService;
        private IAuthorizationService _authorizationService;
        public Edit(IInterestAssetService interestAssetService, IAuthorizationService authorizationService)
        {
            _interestAssetService = interestAssetService;
            _authorizationService = authorizationService;
        }

        [HttpPut("bankSaving/{bankSavingId}")]
        public override async Task<ActionResult<BankingAssetResponse>> HandleAsync(
            [FromMultipleSource] EditBankingAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.EditBankAssetCommand.Adapt<EditBankSavingAssetDto>();
            var result = _interestAssetService.EditBankSavingAsset(request.PortfolioId, request.BankSavingId, dto);

            if (result is null)
                return NotFound("Could not find the asset");
            return Ok(result.Adapt<BankingAssetResponse>());
        }
    }
}