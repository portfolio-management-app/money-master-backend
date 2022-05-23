using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class Edit: BasePortfolioRelatedEndpoint<EditCryptoRequest, CryptoResponse>
    {
        private readonly ICryptoService _cryptoService;
        private readonly IAuthorizationService _authorizationService;

        public Edit(ICryptoService cryptoService, IAuthorizationService authorizationService)
        {
            _cryptoService = cryptoService;
            _authorizationService = authorizationService;
        }
    
        [HttpPut("crypto/{cryptoId}")]
        public override async Task<ActionResult<CryptoResponse>> HandleAsync([FromMultipleSource]EditCryptoRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
            {
                return Unauthorized(); 
            }

            var dto = request.EditCryptoCommand.Adapt<EditCryptoDto>();
            var result = _cryptoService.EditCrypto(request.CryptoId, dto);
            if (result is null)
                return NotFound(); 
            

            return Ok(result.Adapt<CryptoResponse>()); 
        }
    }
}