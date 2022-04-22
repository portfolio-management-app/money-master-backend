using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.CryptoCurrency
{
    [Authorize]
    [Route("/portfolio/{portfolioId}")]
    public class Create : EndpointBaseAsync.WithRequest<CreateNewCryptoCurrencyAssetRequest>.WithActionResult<
        CryptoCurrencyResponse>
    {
        private readonly ICryptoService _cryptoService;

        public Create(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpPost("crypto")]
        public override async Task<ActionResult<CryptoCurrencyResponse>> HandleAsync(
            [FromMultipleSource] CreateNewCryptoCurrencyAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            var dto = request.CreateNewCryptoCurrencyCommand.Adapt<CryptoDto>();
            var createdCrypto = await _cryptoService.CreateNewCryptoAsset(request.PortfolioId, dto);
            return Ok(createdCrypto.Adapt<CryptoCurrencyResponse>());
        }
    }
}