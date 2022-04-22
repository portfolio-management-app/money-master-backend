using System;
using System.Collections.Generic;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    [Authorize]
    [Route("personalAsset/interest")]
    public class GetList : EndpointBaseSync.WithRequest<int>.WithActionResult<List<object>>
    {
        private readonly IInterestAssetService _interestAssetService;

        public GetList(IInterestAssetService interestAssetService)
        {
            _interestAssetService = interestAssetService;
        }

        [HttpGet("custom/{customInfoId}")]
        public override ActionResult<List<object>> Handle(int customInfoId)

        {
            try
            {
                var userId = (int)HttpContext.Items["userId"]!;
                var listCustomAsset =
                    _interestAssetService.GetAllUserCustomInterestAssetInCategory(userId, customInfoId);
                return Ok(listCustomAsset.Adapt<List<SingleCustomInterestAssetResponse>>());
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}