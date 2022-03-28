using Microsoft.AspNetCore.Authorization;

namespace PublicAPI.AuthorizationsPolicies
{
    public class IsPortfolioOwnerRequirement : IAuthorizationRequirement
    {
    }
}