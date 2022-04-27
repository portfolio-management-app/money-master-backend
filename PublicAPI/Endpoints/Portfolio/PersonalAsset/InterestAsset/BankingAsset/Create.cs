using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate;
using ApplicationCore.AssetAggregate.BankSavingAssetAggregate.DTOs;
using ApplicationCore.TransactionAggregate;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
  
    public class Create : BasePortfolioRelatedEndpoint<CreateNewBankingAssetRequest,BankingAssetResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssetTransactionService _transactionService;
        private readonly IBankSavingService _bankSavingService;

        public Create( IAuthorizationService authorizationService, IAssetTransactionService transactionService, IBankSavingService bankSavingService)
        {
            _authorizationService = authorizationService;
            _transactionService = transactionService;
            _bankSavingService = bankSavingService;
        }

        [HttpPost("bankSaving")]
        public override async Task<ActionResult<BankingAssetResponse>> HandleAsync([FromMultipleSource]CreateNewBankingAssetRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return  Unauthorized(NotAllowedPortfolioMessage);
            var dto = request.CreateNewBankingAssetCommand.Adapt<CreateNewBankSavingAssetDto>();
            var newBankSavingAsset = _bankSavingService.AddBankSavingAsset(request.PortfolioId, dto);
            _ = _transactionService.AddCreateNewAssetTransaction(newBankSavingAsset,
                newBankSavingAsset.InputMoneyAmount, newBankSavingAsset.InputCurrency);
            return newBankSavingAsset.Adapt<BankingAssetResponse>();
        }
    }
}