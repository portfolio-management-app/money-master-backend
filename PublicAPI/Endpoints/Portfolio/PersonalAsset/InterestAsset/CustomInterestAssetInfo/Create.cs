using System;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomInterestAssetInfo
{
    [Route("personalAsset/interest")]
    [Authorize]
    public class Create : EndpointBaseSync.WithRequest<CreateCustomInterestAssetInfoRequest>.WithActionResult<
        CreateCustomInterestAssetInfoResponse>
    {
        private readonly IInterestAssetService _interestAssetService;

        public Create(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }

        [HttpPost("custom")]
        public override ActionResult<CreateCustomInterestAssetInfoResponse> Handle([FromMultipleSource]
            CreateCustomInterestAssetInfoRequest request)
        {
            var userId = (int)HttpContext.Items["userId"]!;
            try
            {
                var result =
                    _interestAssetService.AddCustomInterestAssetInfo(userId, request.Name);
                return result.Adapt<CreateCustomInterestAssetInfoResponse>();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}