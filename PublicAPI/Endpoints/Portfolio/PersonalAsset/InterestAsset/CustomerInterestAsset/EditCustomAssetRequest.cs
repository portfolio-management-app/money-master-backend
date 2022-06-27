using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{

    public class EditCustomAssetCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
    }
    public class EditCustomAssetRequest
    {
        [FromBody] public EditCustomAssetCommand EditCustomAssetCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
        [FromRoute] public int AssetId { get; set; }
    }
}