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
  
    public class Create : BasePortfolioRelatedEndpoint<CreateNewBankingAssetRequest,BankingAssetResponse>
    {
        private readonly IInterestAssetService _interestAssetService;
        private readonly IAuthorizationService _authorizationService;

        public Create(IInterestAssetService interestAssetService, IAuthorizationService authorizationService)
        {
            _interestAssetService = interestAssetService;
            _authorizationService = authorizationService;
        }

        [HttpPost("bankSaving")]
        public override async Task<ActionResult<BankingAssetResponse>> HandleAsync(CreateNewBankingAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.CreateNewBankingAssetCommand.Adapt<CreateNewBankSavingAssetDto>();
            var newBankSavingAsset = _interestAssetService.AddBankSavingAsset(request.PortfolioId, dto);
            return newBankSavingAsset.Adapt<BankingAssetResponse>();
        }
    }
}