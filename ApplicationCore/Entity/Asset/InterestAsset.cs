using System;

namespace ApplicationCore.Entity.Asset
{
    public class InterestAsset : PersonalAsset
    {
        public decimal InterestRate { get; set; }
        public int TermRange { get; set; } // in day 

        protected void Update(string name, DateTime inputDay, decimal inputMoneyAmount
            , string inputCurrency, string description, decimal interestRate, int termRange)
        {
            base.Update(name,inputDay,inputMoneyAmount,inputCurrency,description);
            InterestRate = interestRate;
            TermRange = termRange;
        }
        // TODO: deal with change interest rate (continue with the current amount or reset from start)
    }
}