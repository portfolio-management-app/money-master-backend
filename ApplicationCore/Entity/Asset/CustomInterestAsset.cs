using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public class CustomInterestAsset : InterestAsset
    {
        public CustomInterestAssetInfo CustomInterestAssetInfo { get; set; }
        public int CustomInterestAssetInfoId { get; set; }

        public override string GetAssetType()
        {
            return "custom";
        }

        public override Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository,
            IStockPriceRepository stockPriceRepository)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> WithdrawAll()
        {
            InputMoneyAmount = decimal.Zero;
            return true;
        }
    }
}