using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            var userId = int.Parse(HttpContext.Items["userId"].ToString() ?? string.Empty);
            var foundCategories = _interestAssetService.GetAllUserCustomInterestAssetCategory(userId);

            return Ok(foundCategories.Adapt<List<CreateCustomInterestAssetInfoResponse>>());
        }

    }
}