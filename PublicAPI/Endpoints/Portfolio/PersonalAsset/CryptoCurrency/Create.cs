using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    [Authorize]
    [Route("/portfolio/{portfolioId}")]
    public class Create : BasePortfolioRelatedEndpoint<CreateNewCryptoCurrencyAssetRequest,CryptoCurrencyResponse>
    {
        private readonly ICryptoService _cryptoService;
        private readonly IAuthorizationService _authorizationService;

        public Create(ICryptoService cryptoService, IAuthorizationService authorizationService)
        {
            _cryptoService = cryptoService;
            _authorizationService = authorizationService;
        }

        [HttpPost("crypto")]
        public override async Task<ActionResult<CryptoCurrencyResponse>> HandleAsync(
            [FromMultipleSource] CreateNewCryptoCurrencyAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.CreateNewCryptoCurrencyCommand.Adapt<CryptoDto>();
            var createdCrypto = await _cryptoService.CreateNewCryptoAsset(request.PortfolioId, dto);
            return Ok(createdCrypto.Adapt<CryptoCurrencyResponse>());
        }
    }
}