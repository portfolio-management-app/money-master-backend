using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset
{
    public class PortfolioResourceRequest
    {
        [FromRoute] public int AssetId { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}