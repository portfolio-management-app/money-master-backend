using System;

namespace ApplicationCore.Entity.Transactions
{
    public class Transaction : BaseEntity
    {
        public int Id { get; set; }

        public decimal? AmountOfReferentialAssetBeforeCreatingTransaction { get; set; }
        public int? ReferentialAssetId { get; set; }
        public string ReferentialAssetType { get; set; }

        public string ReferentialAssetName { get; set; }


        public decimal Amount { get; set; }
        public decimal? AmountInDestinationAssetUnit { get; set; }
        public string CurrencyCode { get; set; }

        public decimal? Fee { get; set; }
        public decimal? Tax { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastChanged { get; set; } = DateTime.Now;
        public Portfolio Portfolio { get; set; } 
        public int PortfolioId { get; set; }

        public Transaction(string referentialAssetType, int referentialAssetId, decimal amount,
            string currencyCode)
        {
            ReferentialAssetId = referentialAssetId;
            ReferentialAssetType = referentialAssetType;
            CurrencyCode = currencyCode;
            Amount = amount;
            CreatedAt = DateTime.Now;
            LastChanged = DateTime.Now;
        }

        public Transaction()
        {
        }
    }
}