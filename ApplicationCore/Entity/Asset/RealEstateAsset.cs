using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public class RealEstateAsset : PersonalAsset
    {
        public string InputCurrency { get; set; }
        public decimal InputMoneyAmount { get; set; }
        public decimal CurrentPrice { get; set; }
    

        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository)
        {
            var rateObj = await currencyRateRepository.GetRateObject(InputCurrency);
            return rateObj.GetValue(destinationCurrencyCode) * CurrentPrice; 
        }
    }
}