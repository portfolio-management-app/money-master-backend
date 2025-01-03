using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ReportAggregate.Models;
using ApplicationCore.ReportAggregate.Visitors;

namespace ApplicationCore.Entity.Asset
{
    public abstract class PersonalAsset : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InputDay { get; set; } = DateTime.Now;
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public string Description { get; set; }

        protected void Update(string name, DateTime inputDay,
            string description)
        {
            Name = name;
            InputDay = inputDay;
            Description = description;
            LastChanged = DateTime.Now;
        }

        public abstract Task<decimal> CalculateValueInCurrency(string destinationCurrencyCode,
            ExternalPriceFacade priceFacade);

        public abstract string GetAssetType();

        public abstract Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ExternalPriceFacade priceFacade);

        public abstract Task<bool> AddValue(decimal amountInAssetUnit);
        public abstract Task<bool> WithdrawAll();
        public abstract Task<IEnumerable<ProfitLossBasis>> AcceptVisitor(IVisitor visitor, int period);
        public abstract decimal GetAssetSpecificAmount();
        public abstract string GetCurrency();
    }
}