using Microsoft.AspNetCore.Mvc;
using PublicAPI.Attributes;
using PublicAPI.Endpoints.PersonalAsset.InterestAsset.BankingAsset;

namespace PublicAPI.Endpoints.Portfolio.InvestFund
{
    public class AddToInvestFundCommand
    {
        public int ReferentialAssetId { get; set; }

        [CustomAllowedInputValidation(AllowableValues =
            new[] { "bankSaving", "cash", "crypto", "custom", "realEstate", "stock" })]
        public string ReferentialAssetType { get; set; }

        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsTransferringAll { get; set; }
    }

    public class AddToInvestFundRequest
    {
        [FromBody] public AddToInvestFundCommand AddToInvestFundCommand { get; set; }
        [FromRoute] public int PortfolioId { get; set; }
    }
}