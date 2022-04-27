using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class Edit : BasePortfolioRelatedEndpoint<EditBankingAssetRequest, BankingAssetResponse>
    {
        private IAuthorizationService _authorizationService;
        private readonly IBankSavingService _bankSavingService;
        public Edit( IAuthorizationService authorizationService, IBankSavingService bankSavingService)
        {
            _authorizationService = authorizationService;
            _bankSavingService = bankSavingService;
        }

        [HttpPut("bankSaving/{bankSavingId}")]
        public override async Task<ActionResult<BankingAssetResponse>> HandleAsync(
            [FromMultipleSource] EditBankingAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.EditBankAssetCommand.Adapt<EditBankSavingAssetDto>();
            var result = _bankSavingService.EditBankSavingAsset(request.PortfolioId, request.BankSavingId, dto);

            if (result is null)
                return NotFound("Could not find the asset");
            return Ok(result.Adapt<BankingAssetResponse>());
        }
    }
}