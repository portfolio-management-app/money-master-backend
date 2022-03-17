using System;

namespace PublicAPI.Endpoints.PersonalAsset.RealEstate
{
    public class CreateNewRealEstateAssetCommand
    {
        public string Name { get; set; }
        public string BankCode { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public bool IsGoingToReinState { get; set; }
        public double CurrentPrice { get; set; }
    }
}