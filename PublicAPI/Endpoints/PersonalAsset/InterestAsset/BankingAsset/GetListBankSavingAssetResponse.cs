using System;

namespace PublicAPI.Endpoints.PersonalAsset.InterestAsset.BankingAsset
{
    public class GetListBankSavingAssetResponse
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; }
        public DateTime LastChanged { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
        public bool IsGoingToReinState { get; set; }
    }
}