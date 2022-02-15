using System;
using System.Linq;
using ApplicationCore.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomInterestAssetInfo
{
    public class Create: EndpointBaseSync.WithRequest<CreateCustomInterestAssetInfoRequest>.WithActionResult<CreateCustomInterestAssetInfoResponse>
    {
        private readonly IInterestAssetService _interestAssetService;

        public Create(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }
        
        [Authorize]
        [HttpPost("personalAsset/interest/custom")]
        public override ActionResult<CreateCustomInterestAssetInfoResponse> Handle(CreateCustomInterestAssetInfoRequest request)
        {
            var userId = GetUserIdFromToken();
            try
            {
                var result =
                    _interestAssetService.AddCustomInterestAssetInfo(userId, request.Name);
                return result.Adapt<CreateCustomInterestAssetInfoResponse>();
            }
            catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        private int GetUserIdFromToken()
        {
            var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == "ID").Value);
            return userId;
        }
    }
}