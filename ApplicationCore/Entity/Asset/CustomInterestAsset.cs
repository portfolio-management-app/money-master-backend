using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;

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
            ExternalPriceFacade priceFacade)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> WithdrawAll()
        {
            InputMoneyAmount = decimal.Zero;
            return true;
        }

        public override Task<ProfitLossBasis> AcceptVisitor(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}