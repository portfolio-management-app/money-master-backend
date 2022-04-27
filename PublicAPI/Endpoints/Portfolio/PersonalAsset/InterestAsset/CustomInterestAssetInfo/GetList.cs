using System.Collections.Generic;
using ApplicationCore.AssetAggregate.CustomAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomInterestAssetInfo
{
    [Authorize]
    [Route("personalAsset/interest")]
    public class GetList : EndpointBaseSync.WithoutRequest.WithActionResult<List<CreateCustomInterestAssetInfoResponse>>
    {
        private readonly ICustomAssetService _customAssetService;

        public GetList(ICustomAssetService customAssetService)
        {
            _customAssetService = customAssetService;
        }

        [HttpGet("custom")]
        public override ActionResult<List<CreateCustomInterestAssetInfoResponse>> Handle()
        {
            var userId = (int)HttpContext.Items["userId"]!;
            var foundCategories = _customAssetService.GetAllUserCustomInterestAssetCategory(userId);

            return Ok(foundCategories.Adapt<List<CreateCustomInterestAssetInfoResponse>>());
        }
    }
}