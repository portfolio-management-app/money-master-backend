using System;

namespace ApplicationCore.Entity.Asset
{
    public class BankSavingAsset : InterestAsset
    {
        public string BankCode { get; set; }

        public bool IsGoingToReinState { get; set; }

        // Reinstate: continue to keep in asset and keep interest rate 
        // Not Reinstate:  withdraw to cash at the end of term 
        public void Update(string name, DateTime inputDay, decimal inputMoneyAmount
            , string inputCurrency,
            string description,
            decimal interestRate,
            int termRange,
            string bankCode,
            bool isGoingToReinState)
        {
            base.Update(name, inputDay, inputMoneyAmount, inputCurrency, description, interestRate, termRange);
            BankCode = bankCode;
            IsGoingToReinState = isGoingToReinState;
        }
    }
}