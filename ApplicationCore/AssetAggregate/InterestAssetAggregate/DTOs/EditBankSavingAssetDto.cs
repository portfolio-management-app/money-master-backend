using System;

namespace ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs
{
    public class EditBankSavingAssetDto
    {
        public string Name { get; set; }
        public string BankCode { get; set; }
        public DateTime InputDay { get; set; }
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public bool IsGoingToReinState { get; set; }
        public string Description { get; set; }
        public decimal InterestRate { get; set; }
        public string ChangeInterestRateType { get; set; } = "CONTINUE_WITH_RATE";  
        public int TermRange { get; set; } // in day 
    }
}