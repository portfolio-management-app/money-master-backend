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
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository, IStockPriceRepository stockPriceRepository)
        {
            var ratesObj = await currencyRateRepository.GetRateObject("USD");
            var priceInUsdDto = await stockPriceRepository.GetPrice(StockCode);

            return CurrentAmountHolding * ratesObj.GetValue(destinationCurrencyCode) * priceInUsdDto.CurrentPrice;
        }

        public override string GetAssetType()
        {
            return "stock";
        }

        public override Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ICurrencyRateRepository currencyRateRepository)
        {
            throw new System.NotImplementedException();
        }
    }
}