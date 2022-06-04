using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{

    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, CashResponse>
    {
        private readonly ICashService _cashService;


        public Get(ICashService cashService)
        {
            _cashService = cashService;


        }

        [HttpGet("cash/{assetId}")]
        public override async Task<ActionResult<CashResponse>> HandleAsync([FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundCash = _cashService.GetById(request.AssetId);
            if (foundCash is null)
                return NotFound();

            return Ok(foundCash.Adapt<CashResponse>());
        }
    }

}
