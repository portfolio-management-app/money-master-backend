using System;
using System.Linq;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    
    [Authorize]
    [Route("portfolio/{portfolioId}")]
    
    public class Create: EndpointBaseSync.WithRequest<CreateCustomInterestAssetRequest>
        .WithActionResult<CreateCustomInterestAssetResponse>
    {
        private readonly IInterestAssetService _interestAssetService;
        public Create(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }
        
        [HttpPost("custom/{customInfoId}")]
        public override ActionResult<CreateCustomInterestAssetResponse> Handle
            ([FromMultipleSource]CreateCustomInterestAssetRequest request)
        {
            try
            {
                
                var userId = (int) HttpContext.Items["userId"]!;
                var dto = request.CustomInterestAssetCommand.Adapt<CreateNewCustomInterestAssetDto>();
                var newAsset = 
                    _interestAssetService.AddCustomInterestAsset(userId,request.CustomInterestAssetInfoId,request.PortfolioId,dto);
                return Ok(newAsset.Adapt<CreateCustomInterestAssetResponse>());
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        

    }
}