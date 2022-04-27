using System;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
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
        private readonly ICustomAssetService _customAssetService;

        public Create(ICustomAssetService customAssetService)
        {
            _customAssetService = customAssetService;
        }

        [HttpPost("custom")]
        public override ActionResult<CreateCustomInterestAssetInfoResponse> Handle(
            CreateCustomInterestAssetInfoRequest request)
        {
            var userId = (int)HttpContext.Items["userId"]!;
            try
            {
                var result =
                    _customAssetService.AddCustomInterestAssetInfo(userId, request.Name);
                return result.Adapt<CreateCustomInterestAssetInfoResponse>();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}