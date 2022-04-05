using System.Collections.Generic;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.CryptoCurrency
{
    [Route("portfolio/{portfolioId}")]
    public class GetList: EndpointBaseSync.WithRequest<int>.WithActionResult<List<CryptoCurrencyResponse>>
    {
        private readonly ICryptoService _cryptoService;

        public GetList(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("crypto")]
        public override ActionResult<List<CryptoCurrencyResponse>> Handle(int portfolioId)
        {
            return (_cryptoService.GetCryptoAssetByPortfolio(portfolioId)).Adapt<List<CryptoCurrencyResponse>>(); 
        }
    }
}