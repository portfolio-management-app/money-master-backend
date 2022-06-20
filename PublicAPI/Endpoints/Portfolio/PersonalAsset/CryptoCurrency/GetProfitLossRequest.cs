using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class GetProfitLossRequest
    {
        [CustomAllowedInputValidation(AllowableValues = new[] { "day", "week", "month" })]
        [FromQuery]
        public string Period { get; set; }

        [FromRoute] public int AssetId { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}