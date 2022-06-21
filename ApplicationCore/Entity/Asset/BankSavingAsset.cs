using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;

namespace ApplicationCore.Entity.Asset
{
    public class BankSavingAsset : InterestAsset
    {
        public string BankCode { get; set; }

        public bool IsGoingToReinState { get; set; }

        // Reinstate: continue to keep in asset and keep interest rate 
        // Not Reinstate:  withdraw to cash at the end of term 
        public void Update(string name, DateTime inputDay,
            string description,
            decimal inputMoneyAmount,
            string inputCurrency,
            decimal interestRate,
            int termRange,
            string bankCode,
            bool isGoingToReinState)
        {
            base.Update(name, inputDay, description, inputMoneyAmount, inputCurrency, interestRate, termRange);
            BankCode = bankCode;
            IsGoingToReinState = isGoingToReinState;
        }

        public override string GetAssetType()
        {
            return "bankSaving";
        }

        public override async Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade)
        {
            await WithdrawAll();
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
            InputMoneyAmount = 0;
            LastChanged = DateTime.Now;
            return true;
        }

        public override async Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor, int period)
        {
            return await visitor.VisitBankSaving(this, period);
        }

        public override decimal GetAssetSpecificAmount()
        {
            return InputMoneyAmount;
        }

        public override string GetCurrency()
        {
            return InputCurrency;
        }
    }
}