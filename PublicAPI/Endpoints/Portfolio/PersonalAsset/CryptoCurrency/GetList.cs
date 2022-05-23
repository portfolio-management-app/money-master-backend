using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class GetList : BasePortfolioRelatedEndpoint<int,List<CryptoResponse>>
    {
        private readonly ICryptoService _cryptoService;
        private readonly IAuthorizationService _authorizationService;

        public GetList(ICryptoService cryptoService, IAuthorizationService authorizationService)
        {
            _cryptoService = cryptoService;
            _authorizationService = authorizationService;
        }

        [HttpGet("crypto")]
        public override async Task<ActionResult<List<CryptoResponse>>> HandleAsync(int portfolioId,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(portfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            try
            {
                var list = await _cryptoService.ListByPortfolio(portfolioId);
                return list.Adapt<List<CryptoResponse>>();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}