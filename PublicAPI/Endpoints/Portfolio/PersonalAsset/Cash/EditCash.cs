using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class EditCash : BasePortfolioRelatedEndpoint<int, CashResponse>
    {
        [HttpPut("cash/{cashId}")]
        public override Task<ActionResult<CashResponse>> HandleAsync(int cash, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}