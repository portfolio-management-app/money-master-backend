using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.PortfolioAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PublicAPI.AuthorizationsPolicies
{
    public class IsPortfolioOwnerHandler: AuthorizationHandler<IsPortfolioOwnerRequirement, int>
    {
        private readonly IPortfolioService _portfolioService;

        public IsPortfolioOwnerHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPortfolioOwnerRequirement requirement, int portfolioId)
        {
            var userId = int.Parse(context.User.Claims.First(claim => claim.Type == "ID").Value);
            var foundPortfolio = _portfolioService.GetPortfolioById(portfolioId); 
            if(foundPortfolio is null)
                context.Fail();
            if (userId != foundPortfolio!.UserId) 
            {
                context.Fail();
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}