using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.CryptoCurrency
{
    public class EditCryptoCommand
    {
        public string Name { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string Description { get; set; }
    }

    public class EditCryptoRequest
    {
        [FromBody] public EditCryptoCommand EditCryptoCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
        [FromRoute] public int CryptoId { get; set; }
    }
}