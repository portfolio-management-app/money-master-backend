using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class Create : BasePortfolioRelatedEndpoint<CreateCustomInterestAssetRequest,CreateCustomInterestAssetResponse>
    {
        private readonly IInterestAssetService _interestAssetService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssetTransactionService _assetTransactionService;
        public Create(IInterestAssetService interestAssetService, IAuthorizationService authorizationService, IAssetTransactionService assetTransactionService)
        {
            _interestAssetService = interestAssetService;
            _authorizationService = authorizationService;
            _assetTransactionService = assetTransactionService;
        }

        [HttpPost("custom/{customInfoId}")]
        public override async Task<ActionResult<CreateCustomInterestAssetResponse>> HandleAsync([FromMultipleSource]CreateCustomInterestAssetRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            
            try
            {
                var userId = (int)HttpContext.Items["userId"]!;
                var dto = request.CustomInterestAssetCommand.Adapt<CreateNewCustomInterestAssetDto>();
                var newAsset =
                    _interestAssetService.AddCustomInterestAsset(userId, request.CustomInterestAssetInfoId,
                        request.PortfolioId, dto);
                _ = _assetTransactionService.AddCreateNewAssetTransaction(newAsset, newAsset.InputMoneyAmount,
                    newAsset.InputCurrency);
                return Ok(newAsset.Adapt<CreateCustomInterestAssetResponse>());
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}