using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.RealEstate
{
    public class EditRealEstateAssetCommand
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public decimal BuyPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Description { get; set; }
    }

    public class EditRealEstateAssetRequest
    {
        [FromRoute] public int PortfolioId { get; set; }
        [FromRoute] public int RealEstateId { get; set; }
        [FromBody] public EditRealEstateAssetCommand EditRealEstateAssetCommand { get; set; }
    }
}