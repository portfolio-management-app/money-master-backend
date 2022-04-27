using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class Delete : BasePortfolioRelatedEndpoint<PortfolioResourceRequest, CryptoCurrencyResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICryptoService _cryptoService;

        public Delete(IAuthorizationService authorizationService, ICryptoService cryptoService)
        {
            _authorizationService = authorizationService;
            _cryptoService = cryptoService;
        }

        [HttpDelete("crypto/{assetId}")]
        public override async Task<ActionResult<CryptoCurrencyResponse>> HandleAsync(
            [FromMultipleSource] PortfolioResourceRequest request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var crypto = _cryptoService.SetAssetToDelete(request.AssetId);
            return Ok(crypto.Adapt<CryptoCurrencyResponse>());
        }
    }

 
}