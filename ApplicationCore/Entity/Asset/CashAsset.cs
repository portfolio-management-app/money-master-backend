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

        public override string GetAssetType() => "cash";
        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode, ICurrencyRateRepository currencyRateRepository)
        {
            var rateObject = await currencyRateRepository.GetRateObject(currencyCode);
            var rateToWithdraw = rateObject.GetValue(this.CurrencyCode);
            var valueToWithdraw = rateToWithdraw * withdrawAmount;
            if (valueToWithdraw > Amount)
            {
                return false; 
            }

            Amount -= valueToWithdraw;
            return true;
        }
    }
}