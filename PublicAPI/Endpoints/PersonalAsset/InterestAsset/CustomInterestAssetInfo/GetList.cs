using System.Collections.Generic;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomInterestAssetInfo
{
    [Authorize]
    [Route("personalAsset/interest")]
    public class GetList: EndpointBaseSync.WithoutRequest.WithActionResult<List<CreateCustomInterestAssetInfoResponse>>
    {
        private readonly IInterestAssetService _interestAssetService;

        public GetList(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }
        
        [HttpGet("custom")]

        public override ActionResult<List<CreateCustomInterestAssetInfoResponse>> Handle()
        {
            var userId = (int) HttpContext.Items["userId"]!;
            var foundCategories = _interestAssetService.GetAllUserCustomInterestAssetCategory(userId);

            return Ok(foundCategories.Adapt<List<CreateCustomInterestAssetInfoResponse>>());
        }

    }
}