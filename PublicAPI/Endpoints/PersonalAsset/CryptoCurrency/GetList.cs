using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.CryptoCurrency
{
    [Route("portfolio/{portfolioId}")]
    public class GetList : EndpointBaseAsync.WithRequest<int>.WithActionResult<List<CryptoCurrencyResponse>>
    {
        private readonly ICryptoService _cryptoService;

        public GetList(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("crypto")]
        public override async Task<ActionResult<List<CryptoCurrencyResponse>>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var list = _cryptoService.ListByPortfolio(portfolioId);
                return list.Adapt<List<CryptoCurrencyResponse>>();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}