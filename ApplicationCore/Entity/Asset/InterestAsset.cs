using System;
using System.Runtime.InteropServices;
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

        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            return await CalculateValueInPastInCurrency(DateTime.Now, destinationCurrencyCode, priceFacade);
        }

        public async Task<decimal> CalculateValueInPastInCurrency(DateTime dateTime, string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            var passedPeriod = (dateTime - InputDay) / TimeSpan.FromDays(TermRange);
            var passedPeriodInt = (int)passedPeriod;
            var total = (double)InputMoneyAmount
                               * Math.Pow(1 + (double)this.InterestRate, passedPeriodInt);

            if (InputCurrency == destinationCurrencyCode)
            {
                return (decimal)total;
            }
            var rateObj = await priceFacade.CurrencyRateRepository.GetRateObject(InputCurrency);
            total *= (double)rateObj.GetValue(destinationCurrencyCode);

            return (decimal)total; 
        }
    }
}