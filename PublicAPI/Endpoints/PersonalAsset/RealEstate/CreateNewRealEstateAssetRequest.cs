using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    public class CreateNewRealEstateAssetCommand
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public decimal BuyPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Description { get; set; }
    }

    public class CreateNewRealEstateAssetRequest
    {
        [FromBody] public CreateNewRealEstateAssetCommand CreateNewRealEstateAssetCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}