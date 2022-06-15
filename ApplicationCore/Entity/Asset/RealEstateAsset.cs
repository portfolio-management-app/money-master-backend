using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;

namespace ApplicationCore.Entity.Asset
{
    public class RealEstateAsset : PersonalAsset
    {
        public string InputCurrency { get; set; }
        public decimal InputMoneyAmount { get; set; }
        public decimal CurrentPrice { get; set; }


        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            if (destinationCurrencyCode == InputCurrency)
                return InputMoneyAmount;
            var rateObj = await priceFacade.CurrencyRateRepository.GetRateObject(InputCurrency);
            return rateObj.GetValue(destinationCurrencyCode) * CurrentPrice;
        }

        public override string GetAssetType()
        {
            return "realEstate";
        }

        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            await WithdrawAll();
            return true;
        }

        public override async Task<bool> AddValue(decimal amountInAssetUnit)
        {
            if (amountInAssetUnit + CurrentPrice < 0)
                return false;
            CurrentPrice += amountInAssetUnit;
            return true;
        }

        public override async Task<bool> WithdrawAll()
        {
            InputMoneyAmount = decimal.Zero;
            return true;
        }

        public override Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}