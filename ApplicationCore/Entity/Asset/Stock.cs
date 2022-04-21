using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public class Stock : PersonalAsset
    {
        public decimal CurrentAmountHolding { get; set; }
        public string StockCode { get; set; }
        public string MarketCode { get; set; } // NYSE, HOSE
        public decimal PurchasePrice { get; set; }
        public string CurrencyCode { get; set; }
        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository)
        {
            var ratesObj = await currencyRateRepository.GetRateObject(CurrencyCode);
            return CurrentAmountHolding * ratesObj.GetValue(destinationCurrencyCode);

        }

        public override string GetAssetType() => "stock";
        public override Task<bool> Withdraw(decimal withdrawAmount, string currencyCode, ICurrencyRateRepository currencyRateRepository)
        {
            throw new System.NotImplementedException();
        }
    }
}