using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entity.Asset
{
    public class CustomInterestAsset : InterestAsset
    {
        public CustomInterestAssetInfo CustomInterestAssetInfo { get; set; }
        public int CustomInterestAssetInfoId { get; set; }
        public override string GetAssetType() => "custom";
        public override Task<bool> Withdraw(decimal withdrawAmount, string currencyCode, ICurrencyRateRepository currencyRateRepository)
        {
            throw new NotImplementedException();
        }
    }
}