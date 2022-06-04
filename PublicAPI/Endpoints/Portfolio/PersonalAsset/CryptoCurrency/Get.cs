using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{

    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, CryptoResponse>
    {
        private readonly ICryptoService _cryptoService;


        public Get(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;

        }

        [HttpGet("crypto/{assetId}")]
        public override async Task<ActionResult<CryptoResponse>> HandleAsync([FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var foundAsset = _cryptoService.GetById(request.AssetId);
            if (foundAsset is null)
                return NotFound();

            return Ok(foundAsset.Adapt<CryptoResponse>());
        }
    }

}
