using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class GetList : BasePortfolioRelatedEndpoint<int, GetListBankSavingAssetResponse>
    {
        private readonly IBankSavingService _bankSavingService;
        private readonly IAuthorizationService _authorizationService;

        public GetList( IAuthorizationService authorizationService, IBankSavingService bankSavingService)
        {
            _authorizationService = authorizationService;
            _bankSavingService = bankSavingService;
        }

        [HttpGet("bankSaving")]
        public override async Task<ActionResult<GetListBankSavingAssetResponse>> HandleAsync(int portfolioId, CancellationToken cancellationToken = new CancellationToken())
        {
            
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var list = await _bankSavingService.ListByPortfolio(portfolioId);
            return Ok(list.Adapt<List<GetListBankSavingAssetResponse>>());
        }
    }
}