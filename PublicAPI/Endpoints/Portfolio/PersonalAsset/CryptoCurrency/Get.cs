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
using ApplicationCore;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class Get : BasePortfolioRelatedEndpoint<GetListTransactionRequest, CryptoResponse>
    {
        private readonly ICryptoService _cryptoService;

        private readonly ExternalPriceFacade _priceFacade;


        public Get(ICryptoService cryptoService, ExternalPriceFacade priceFacade)
        {
            _cryptoService = cryptoService;
            _priceFacade = priceFacade;
        }

        [HttpGet("crypto/{assetId}")]
        public override async Task<ActionResult<CryptoResponse>> HandleAsync(
            [FromRoute] GetListTransactionRequest request, CancellationToken cancellationToken = new())
        {
            var foundAsset = _cryptoService.GetById(request.AssetId);

            if (foundAsset is null)
                return NotFound();
            if (foundAsset.CurrentPrice == 0)
                foundAsset.CurrentPrice =
                    await _priceFacade.CryptoRateRepository.GetCurrentPriceInCurrency(foundAsset.CryptoCoinCode,
                        foundAsset.CurrencyCode);
            return Ok(foundAsset.Adapt<CryptoResponse>());
        }
    }
}