using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.InvestFundAggregate;
using ApplicationCore.PortfolioAggregate;
using ApplicationCore.TransactionAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    [Authorize]
    [Route("portfolio")]
    public class Create : EndpointBaseAsync.WithRequest<CreatePortfolioRequest>.WithActionResult<PortfolioResponse>
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ICashService _cashService;
        private readonly IInvestFundService _investFundService;
        private readonly IAssetTransactionService _transactionService;

        public Create(IPortfolioService portfolioService,
            ICashService cashService, IInvestFundService investFundService, IAssetTransactionService transactionService)
        {
            _portfolioService = portfolioService;
            _cashService = cashService;
            _investFundService = investFundService;
            _transactionService = transactionService;
        }

        [HttpPost]
        public override async Task<ActionResult<PortfolioResponse>> HandleAsync(CreatePortfolioRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = (int)HttpContext.Items["userId"]!;

            var newPortfolio
                = _portfolioService.CreatePortfolio(userId, request.Name, request.InitialCash, request.InitialCurrency);

            var cashDto = new CashDto()
            {
                Amount = request.InitialCash,
                CurrencyCode = request.InitialCurrency,
                Description = request.InitialCashDescription,
                InputDay = DateTime.Now,
                Name = request.InitialCashName,
                PurchasePrice = request.InitialCash,
            };
            var newCash = await _cashService.CreateNewCashAsset(newPortfolio.Id, cashDto);
            _ = _investFundService.AddNewInvestFundToPortfolio(newPortfolio.Id);
                _ = _transactionService.AddCreateNewAssetTransaction(newPortfolio.Id,newCash ,
                                newCash.Amount,
                                newCash.CurrencyCode, false, false, null, null,
                                null);
            return Ok(newPortfolio.Adapt<PortfolioResponse>());
        }
    }
}