using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.PersonalAsset.Cash
{
    [Route("portfolio/{portfolioId}")]
    [Authorize]
    public class Create: EndpointBaseAsync.WithRequest<CreateCashRequest>.WithActionResult<CashResponse>
    {
        private readonly ICashService _cashService;

        public Create(ICashService cashService)
        {
            _cashService = cashService;
        }

        [HttpPost("cash")]
        public override Task<ActionResult<CashResponse>> HandleAsync([FromMultipleSource]CreateCashRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var dto = request.CreateCashCommand.Adapt<CashDto>();
            CashAsset newCashAsset = _cashService.CreateNewCashAsset(request.PortfolioId, dto); 
            return Task.FromResult<ActionResult<CashResponse>>(Ok(newCashAsset.Adapt<CashResponse>()));
        }
    }
}