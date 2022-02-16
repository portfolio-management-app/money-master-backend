using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class CreateCustomInterestAssetCommand
    {
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day    
    }

    public class CreateCustomInterestAssetRequest
    {
        [FromRoute(Name = "customInfoId")] public int CustomInterestAssetInfoId { get; set; } 
        [FromBody] public CreateCustomInterestAssetCommand CustomInterestAssetCommand { get; set; }
    }
}