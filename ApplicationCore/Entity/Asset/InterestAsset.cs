using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public abstract class InterestAsset : PersonalAsset
    {
        public decimal InputMoneyAmount { get; set; }
        public string InputCurrency { get; set; }
        public decimal InterestRate { get; set; }
        public int TermRange { get; set; } // in day 

        protected void Update(string name, DateTime inputDay,
            string description, decimal inputMoneyAmount, string inputCurrency, decimal interestRate, int termRange)
        {
            base.Update(name, inputDay, description);
            InterestRate = interestRate;
            TermRange = termRange;
            InputMoneyAmount = inputMoneyAmount;
            InputCurrency = inputCurrency;
        }

        // TODO: deal with change interest rate (continue with the current amount or reset from start)
        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository, IStockPriceRepository stockPriceRepository)
        {
            var rateObj = await currencyRateRepository.GetRateObject(InputCurrency);
            return rateObj.GetValue(destinationCurrencyCode) * InputMoneyAmount;
        }
    }
}