using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Entity.Asset
{
    public class Crypto: PersonalAsset
    {
        
        public decimal PurchasePrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string CryptoCoinCode { get; set; }
        public decimal CurrentPrice { get; set; }
        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository)
        {
            return await cryptoRateRepository.GetCurrentPrice(CryptoCoinCode, destinationCurrencyCode) 
                   * CurrentAmountHolding ;
        }
    }
}