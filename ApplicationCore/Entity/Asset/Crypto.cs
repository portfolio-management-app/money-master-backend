using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Entity.Asset
{
    public class Crypto : PersonalAsset
    {
        public decimal PurchasePrice { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentAmountHolding { get; set; }
        public string CryptoCoinCode { get; set; }
        public decimal CurrentPrice { get; set; }

        public override async Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade)
        {
            var currentPriceInCurrency =
                await priceFacade.CryptoRateRepository.GetCurrentPriceInCurrency(CryptoCoinCode,
                    destinationCurrencyCode);
            return currentPriceInCurrency * CurrentAmountHolding;
        }

        public override string GetAssetType()
        {
            return "crypto";
        }

        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            var currentValue =
                await CalculateValueInCurrency(currencyCode,
                    priceFacade);
            if (currentValue < withdrawAmount)
                return false;
            CurrentAmountHolding -= withdrawAmount * CurrentAmountHolding / currentValue;
            return true;
        }

        public override async Task<bool> AddValue(decimal amountInAssetUnit)
        {
            if (CurrentAmountHolding + amountInAssetUnit < 0)
                return false;
            CurrentAmountHolding += amountInAssetUnit;
            return true;
        }

        public override async Task<bool> WithdrawAll()
        {
            CurrentAmountHolding = decimal.Zero;
            return true;
        }

        public override async Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor)
        {
            return await visitor.VisitCrypto(this);
        }

        public override decimal GetAssetSpecificAmount()
        {
            return this.CurrentAmountHolding; 
        }

        public override string GetCurrency()
        {
            return this.CurrencyCode; 
        }
    }
}