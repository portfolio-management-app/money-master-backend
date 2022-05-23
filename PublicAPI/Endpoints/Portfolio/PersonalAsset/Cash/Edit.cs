using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class Edit: BasePortfolioRelatedEndpoint<EditCashRequest,CashResponse>
    {
        private IAuthorizationService _authorizationService;
        private readonly ICashService _cashService;

        public Edit(IAuthorizationService authorizationService, ICashService cashService)
        {
            _authorizationService = authorizationService;
            _cashService = cashService;
        }

        [HttpPut("cash/{cashId}")]
        public override async Task<ActionResult<CashResponse>> HandleAsync([FromMultipleSource]EditCashRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
            {
                return Unauthorized(); 
            }
            var dto = request.EditCashCommand.Adapt<EditCashDto>();
            var result = _cashService.EditCash(request.CashId, dto);

            if (result is null)
                return NotFound();
            return result.Adapt<CashResponse>(); 
        }
    }
}