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
    public class Delete : BasePortfolioRelatedEndpoint<PortfolioAssetRequest, CryptoResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICryptoService _cryptoService;

        public Delete(IAuthorizationService authorizationService, ICryptoService cryptoService)
        {
            _authorizationService = authorizationService;
            _cryptoService = cryptoService;
        }

        [HttpDelete("crypto/{assetId}")]
        public override async Task<ActionResult<CryptoResponse>> HandleAsync(
            [FromMultipleSource] PortfolioAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var crypto = _cryptoService.SetAssetToDelete(request.AssetId);
            return Ok(crypto.Adapt<CryptoResponse>());
        }
    }
}