using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{

    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, RealEstateResponse>
    {
        private readonly IRealEstateService _realEstateService;


        public Get(IRealEstateService realEstateService)
        {
            _realEstateService=realEstateService;

        }

        [HttpGet("realEstate/{assetId}")]
        public override async Task<ActionResult<RealEstateResponse>> HandleAsync([FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundAsset = _realEstateService.GetById(request.AssetId);
            if (foundAsset is null)
                return NotFound();

            return Ok(foundAsset.Adapt<RealEstateResponse>());
        }
    }

}