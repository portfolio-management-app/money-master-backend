using System;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.BankingAsset
{
    public class CreateNewBankingAssetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BankCode { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public bool IsGoingToReinState { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
    }
}