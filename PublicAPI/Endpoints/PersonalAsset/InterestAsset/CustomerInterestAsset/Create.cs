using System;
using System.Linq;
using ApplicationCore.InterestAssetAggregate;
using ApplicationCore.InterestAssetAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    
    [Authorize]
    [Route("personalAsset/interest")]
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
                var dto = request.CustomInterestAssetCommand.Adapt<CreateNewCustomInterestAssetDto>();
                var newAsset = 
                    _interestAssetService.AddCustomInterestAsset(GetUserIdFromToken(),request.CustomInterestAssetInfoId,dto);
                return Ok(newAsset.Adapt<CreateCustomInterestAssetResponse>());
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        
        private int GetUserIdFromToken()
        {
            var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == "ID").Value);
            return userId;
        }
    }
}