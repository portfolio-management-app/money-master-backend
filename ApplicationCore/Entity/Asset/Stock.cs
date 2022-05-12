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
        public decimal? CurrentPrice { get; set; } = null;


        public async Task<decimal> GetCurrentPricePerUnit( ExternalPriceFacade priceFacade)
        {
            var priceInUsdDto = await priceFacade.StockPriceRepository.GetPrice(StockCode);
            if (CurrencyCode == "USD") return priceInUsdDto.CurrentPrice;
            var ratesObj = await priceFacade.CurrencyRateRepository.GetRateObject("USD");

           return ratesObj.GetValue(CurrencyCode) * priceInUsdDto.CurrentPrice;

        }

        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            var priceInUsdDto = await priceFacade.StockPriceRepository.GetPrice(StockCode);
            if (destinationCurrencyCode == "USD") return priceInUsdDto.CurrentPrice * CurrentAmountHolding;
            var ratesObj = await priceFacade.CurrencyRateRepository.GetRateObject("USD");

            return CurrentAmountHolding * ratesObj.GetValue(destinationCurrencyCode) * priceInUsdDto.CurrentPrice;
        }

        public override string GetAssetType()
        {
            return "stock";
        }

        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            var currentValue =
                await CalculateValueInCurrency(currencyCode,priceFacade);
            if (currentValue < withdrawAmount)
                return false;
            CurrentAmountHolding -= withdrawAmount * CurrentAmountHolding / currentValue;
            return true;
        }

        public override async Task<bool> WithdrawAll()
        {
            CurrentAmountHolding = decimal.Zero;
            return true;
        }
    }
}