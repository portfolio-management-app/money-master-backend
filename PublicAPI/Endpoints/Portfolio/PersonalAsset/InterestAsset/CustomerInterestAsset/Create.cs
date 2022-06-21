using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using ApplicationCore.AssetAggregate.CustomAssetAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class Create : BasePortfolioRelatedEndpoint<CreateCustomInterestAssetRequest,
        CreateCustomInterestAssetResponse>
    {
        private readonly ICustomAssetService _customAssetService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssetTransactionService _assetTransactionService;

        public Create(ICustomAssetService customAssetService, IAuthorizationService authorizationService,
            IAssetTransactionService assetTransactionService)
        {
            _customAssetService = customAssetService;
            _authorizationService = authorizationService;
            _assetTransactionService = assetTransactionService;
        }

        [HttpPost("custom/{customInfoId}")]
        public override async Task<ActionResult<CreateCustomInterestAssetResponse>> HandleAsync(
            [FromMultipleSource] CreateCustomInterestAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);

            try
            {
                var userId = (int)HttpContext.Items["userId"]!;
                var dto = request.CustomInterestAssetCommand.Adapt<CreateNewCustomInterestAssetDto>();
                var newAsset =
                    await _customAssetService.AddCustomInterestAsset(userId, request.CustomInterestAssetInfoId,
                        request.PortfolioId, dto);
                _ = _assetTransactionService.AddCreateNewAssetTransaction(request.PortfolioId, newAsset,
                    newAsset.InputMoneyAmount,
                    newAsset.InputCurrency, dto.IsUsingInvestFund, dto.IsUsingCash, dto.UsingCashId, dto.Fee, dto.Tax);
                return Ok(newAsset.Adapt<CreateCustomInterestAssetResponse>());
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}