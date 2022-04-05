using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.CryptoCurrency
{
    public class CreateNewCryptoCurrencyCommand
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; } 
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public string Description { get; set; }
        public string CryptoCoinCode { get; set; }
    }
    public class CreateNewCryptoCurrencyAssetRequest
    {
        [FromRoute] public int PortfolioId { get; set; }
        [FromBody] public CreateNewCryptoCurrencyCommand CreateNewCryptoCurrencyCommand { get; set; }
    }
}