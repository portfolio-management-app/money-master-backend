using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public class BankSavingAsset : InterestAsset
    {
        public string BankCode { get; set; }

        public bool IsGoingToReinState { get; set; }

        // Reinstate: continue to keep in asset and keep interest rate 
        // Not Reinstate:  withdraw to cash at the end of term 
        public void Update(string name, DateTime inputDay, 
            string description,
            decimal inputMoneyAmount,
            string inputCurrency,
            decimal interestRate,
            int termRange,
            string bankCode,
            bool isGoingToReinState)
        {
            base.Update(name, inputDay, description, inputMoneyAmount, inputCurrency, interestRate, termRange);
            BankCode = bankCode;
            IsGoingToReinState = isGoingToReinState;
        }

        public override string GetAssetType() => "bankSaving";
        public override Task<bool> Withdraw(decimal withdrawAmount, string currencyCode, ICurrencyRateRepository currencyRateRepository)
        {
            throw new NotImplementedException(); 
        }
    }
}