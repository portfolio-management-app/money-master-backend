using System;
using System.Net.Http;
using ApplicationCore.AssetAggregate.CashAggregate;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.InvestFundAggregate;
using ApplicationCore.PortfolioAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio
{
    [Authorize]
    [Route("portfolio")]
    public class Create : EndpointBaseSync.WithRequest<CreatePortfolioRequest>.WithActionResult<CreatePortfolioResponse>
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IInvestFundService _investFundService;
        private readonly ICashService _cashService;

        public Create(IPortfolioService portfolioService, IInvestFundService investFundService, ICashService cashService)
        {
            _portfolioService = portfolioService;
            _investFundService = investFundService;
            _cashService = cashService;
        }

        [HttpPost]
        public override ActionResult<CreatePortfolioResponse> Handle(CreatePortfolioRequest request)
        {
            var userId = (int)HttpContext.Items["userId"]!;

            var newPortfolio
                = _portfolioService.CreatePortfolio(userId, request.Name, request.InitialCash, request.InitialCurrency);

            var cashDto = new CashDto()
            {
                Amount = request.InitialCash,
                CurrencyCode = request.InitialCurrency,
                Description = "New cash when create portfolio",
                InputDay = DateTime.Now,
                Name = "Initial Cash",
                PurchasePrice = request.InitialCash
            };
            _ = _cashService.CreateNewCashAsset(newPortfolio.Id, cashDto);
            return Ok(newPortfolio.Adapt<CreatePortfolioResponse>());
        }
    }
}