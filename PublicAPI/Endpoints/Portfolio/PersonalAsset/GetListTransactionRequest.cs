using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.Cash
{
    public class GetListTransactionRequest
    {
        [FromRoute] public int AssetId { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}