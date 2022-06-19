using System;
using System.Collections.Generic;
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


        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            var rateObject = await priceFacade.CurrencyRateRepository.GetRateObject(currencyCode);
            var rateToWithdraw = rateObject.GetValue(InputCurrency);
            var valueToWithdraw = rateToWithdraw * withdrawAmount;
            if (valueToWithdraw > InputMoneyAmount) return false;

            InputMoneyAmount -= valueToWithdraw;
            return true;
        }

        public override async Task<bool> AddValue(decimal amountInAssetUnit)
        {
            if (InputMoneyAmount + amountInAssetUnit < 0)
                return false;
            InputMoneyAmount += amountInAssetUnit;
            return true;
        }


        public override async Task<bool> WithdrawAll()
        {
            InputMoneyAmount = decimal.Zero;
            return true;
        }

        public override Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override decimal GetAssetSpecificAmount()
        {
            return this.InputMoneyAmount; 
        }

        public override string GetCurrency()
        {
            return this.InputCurrency; 
        }
    }
}