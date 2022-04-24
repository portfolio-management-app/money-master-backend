using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
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
        private readonly IAssetTransactionService _transactionService;

        public Create(ICryptoService cryptoService, IAuthorizationService authorizationService, IAssetTransactionService transactionService)
        {
            _cryptoService = cryptoService;
            _authorizationService = authorizationService;
            _transactionService = transactionService;
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
            var currentValue =
                _ = _transactionService.AddCreateNewAssetTransaction(createdCrypto, 
                    createdCrypto.PurchasePrice * createdCrypto.CurrentAmountHolding,
                    createdCrypto.CurrencyCode);
            return Ok(createdCrypto.Adapt<CryptoCurrencyResponse>());
        }
    }
}