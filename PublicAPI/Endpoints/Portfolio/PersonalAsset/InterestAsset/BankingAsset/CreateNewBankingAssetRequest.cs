using System;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.BankingAsset
{
    public class CreateNewBankingAssetCommand : BaseCreateRequest
    {
        public string Name { get; set; }
        public string BankCode { get; set; }
        public DateTime InputDay { get; set; }
        public double InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; } = "VND";
        public bool IsGoingToReinState { get; set; }
        public string Description { get; set; }
        public double InterestRate { get; set; }
        public int TermRange { get; set; } // in day 
        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }
    }

    public class CreateNewBankingAssetRequest
    {
        [FromBody] public CreateNewBankingAssetCommand CreateNewBankingAssetCommand { get; set; }
        [FromRoute(Name = "portfolioId")] public int PortfolioId { get; set; }
    }
}