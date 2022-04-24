using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    
    public class Create : BasePortfolioRelatedEndpoint<CreateCashRequest, CashResponse>
    {
        private readonly ICashService _cashService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssetTransactionService _transactionService;

        public Create(ICashService cashService, IAuthorizationService authorizationService, IAssetTransactionService transactionService)
        {
            _cashService = cashService;
            _authorizationService = authorizationService;
            _transactionService = transactionService;
        }

        [HttpPost("cash")]
        public override async Task<ActionResult<CashResponse>> HandleAsync([FromMultipleSource] CreateCashRequest request,
            CancellationToken cancellationToken = new())
        {
            
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            
            
            var dto = request.CreateCashCommand.Adapt<CashDto>();
            var newCashAsset = _cashService.CreateNewCashAsset(request.PortfolioId, dto);

            var transaction =
                _transactionService.AddCreateNewAssetTransaction(newCashAsset, newCashAsset.Amount,
                    newCashAsset.CurrencyCode);
            return Ok(newCashAsset.Adapt<CashResponse>());
        }
    }
}