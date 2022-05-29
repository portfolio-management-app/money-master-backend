using System;

namespace ApplicationCore.AssetAggregate.BankSavingAssetAggregate.DTOs
{
    public class CreateNewBankSavingAssetDto
    {
        public string Name { get; set; }
        public string BankCode { get; set; }
        public DateTime InputDay { get; set; }
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public bool IsGoingToReinState { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
        public bool IsUsingInvestFund { get; set; }
        public bool IsUsingCash { get; set; }
        public int? UsingCashId { get; set; }
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }
}