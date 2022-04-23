using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
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
        public Create(IInterestAssetService interestAssetService, IAuthorizationService authorizationService)
        {
            _interestAssetService = interestAssetService;
            _authorizationService = authorizationService;
        }

        [HttpPost("custom/{customInfoId}")]
        public override async Task<ActionResult<CreateCustomInterestAssetResponse>> HandleAsync(CreateCustomInterestAssetRequest request,
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
                return Ok(newAsset.Adapt<CreateCustomInterestAssetResponse>());
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}