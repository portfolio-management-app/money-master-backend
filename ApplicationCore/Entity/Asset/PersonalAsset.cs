using System;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using ApplicationCore.Interfaces;

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
            ICurrencyRateRepository currencyRateRepository, ICryptoRateRepository cryptoRateRepository, IStockPriceRepository stockPriceRepository);

        public abstract string GetAssetType();

        public abstract Task<bool> Withdraw(decimal withdrawAmount, string currencyCode,
            ICurrencyRateRepository currencyRateRepository);
    }
}