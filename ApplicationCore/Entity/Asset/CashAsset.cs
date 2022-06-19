using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;

namespace ApplicationCore.Entity.Asset
{
    public class CashAsset : PersonalAsset
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }

        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            if (destinationCurrencyCode == CurrencyCode)
                return Amount;
            var rateObj = await priceFacade.CurrencyRateRepository.GetRateObject(CurrencyCode);
            return Amount * rateObj.GetValue(destinationCurrencyCode);
        }

        public override string GetAssetType()
        {
            return "cash";
        }

        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            var rateObject = await priceFacade.CurrencyRateRepository.GetRateObject(currencyCode);
            var rateToWithdraw = rateObject.GetValue(CurrencyCode);
            var valueToWithdraw = rateToWithdraw * withdrawAmount;
            if (valueToWithdraw > Amount) return false;

            Amount -= valueToWithdraw;
            return true;
        }

        public override async Task<bool> AddValue(decimal amountInAssetUnit)
        {
            if (Amount + amountInAssetUnit < 0)
                return false;
            Amount += amountInAssetUnit;
            return true;
        }

        public override async Task<bool> WithdrawAll()
        {
            Amount = 0;
            return true;
        }

        public override Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override decimal GetAssetSpecificAmount()
        {
            return this.Amount; 
        }

        public override string GetCurrency()
        {
            return this.CurrencyCode;
        }
    }
}