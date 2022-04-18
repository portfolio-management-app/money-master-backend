using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public class CashAsset : PersonalAsset
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository)
        {
            var rateObj = await currencyRateRepository.GetRateObject(CurrencyCode);
            return Amount * rateObj.GetValue(destinationCurrencyCode); 
        }
    }
}