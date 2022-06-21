using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.InvestFundAggregate;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Endpoints.Portfolio.Transactions;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class GetInvestFundTransactions : BasePortfolioRelatedEndpoint<GetInvestFundTransactionRequest, List<InvestFundTransactionResponse>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IInvestFundService _investFundService;

        public GetInvestFundTransactions(IAuthorizationService authorizationService,
            IInvestFundService investFundService)
        {
            _authorizationService = authorizationService;
            _investFundService = investFundService;
        }


        [HttpGet("investFund/transactions")]
        public override async Task<ActionResult<List<InvestFundTransactionResponse>>> HandleAsync
            ([FromMultipleSource] GetInvestFundTransactionRequest request, CancellationToken cancellationToken = new())
        {
            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
                return Unauthorized("You are allowed for this portfolio");
            var listTransactions = _investFundService.GetInvestFundTransactionByPortfolio(request.PortfolioId, request.PageNumber, request.PageSize, request.StartDate, request.EndDate, request.Type);
            return Ok(listTransactions.Select(trans => new TransactionResponse(trans)));
        }
    }
}