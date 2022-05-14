using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.InvestFundAggregate;
using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class GetInvestFundTransactions: BasePortfolioRelatedEndpoint<int,List<InvestFundTransactionResponse>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IInvestFundService _investFundService;
        public GetInvestFundTransactions(IAuthorizationService authorizationService, IInvestFundService investFundService)
        {
            _authorizationService = authorizationService;
            _investFundService = investFundService;
        }
        
        
        [HttpGet("investFund/transactions")]
        public override async Task<ActionResult<List<InvestFundTransactionResponse>>> HandleAsync
            ([FromRoute]int portfolioId , CancellationToken cancellationToken = new CancellationToken())
        {
            if (! await IsAllowedToExecute(portfolioId, _authorizationService))
            {
                return Unauthorized("You are allowed for this portfolio"); 
            }

            var listTransactions = _investFundService.GetInvestFundTransactionByPortfolio(portfolioId);
            var listResponse = listTransactions.Adapt<List<InvestFundTransactionResponse>>();
            return Ok(listResponse); 
        }
    }
}