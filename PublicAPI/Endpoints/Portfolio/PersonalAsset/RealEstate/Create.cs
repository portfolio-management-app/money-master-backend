using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class Create : BasePortfolioRelatedEndpoint<CreateNewRealEstateAssetRequest, RealEstateResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRealEstateService _realEstateService;
        private readonly IAssetTransactionService _transactionService;

        public Create(IAuthorizationService authorizationService, IRealEstateService realEstateService, IAssetTransactionService transactionService)
        {
            _authorizationService = authorizationService;
            _realEstateService = realEstateService;
            _transactionService = transactionService;
        }

        [HttpPost("realEstate")]
        public override async Task<ActionResult<RealEstateResponse>> HandleAsync(
            [FromMultipleSource] CreateNewRealEstateAssetRequest request,
            CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.CreateNewRealEstateAssetCommand.Adapt<RealEstateDto>();
            var newRealEstate = _realEstateService.CreateNewRealEstateAsset(request.PortfolioId, dto);
            _ = _transactionService.AddCreateNewAssetTransaction(newRealEstate, newRealEstate.InputMoneyAmount,
                newRealEstate.InputCurrency);
            return Ok(newRealEstate.Adapt<RealEstateResponse>());
        }
    }
}