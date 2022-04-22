using System;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class EditBankAssetCommand
    {
        public string Name { get; set; }
        public string BankCode { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public bool IsGoingToReinState { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }

        [CustomAllowedInputValidation(AllowableValues = new[] { "CONTINUE_WITH_RATE", "RESET_TERM_RATE" })]
        public string ChangeInterestRateType { get; set; } = "CONTINUE_WITH_RATE";

        public int TermRange { get; set; } // in day 
    }

    public class EditBankingAssetRequest
    {
        [FromBody] public EditBankAssetCommand EditBankAssetCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
        [FromRoute] public int BankSavingId { get; set; }
    }
}