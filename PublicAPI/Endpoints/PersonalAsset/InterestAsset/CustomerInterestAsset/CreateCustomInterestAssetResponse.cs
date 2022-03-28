using System;
using PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomInterestAssetInfo;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class CreateCustomInterestAssetResponse
    {
        public CreateCustomInterestAssetInfoResponse CustomInterestAssetInfo { get; set; }

        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day    
    }
}