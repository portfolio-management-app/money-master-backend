using System;

namespace ApplicationCore.Entity.Transactions
{
    public class Transaction: BaseEntity
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; } = TransactionType.UserEditAsset;
        public string Description { get; set; }
        public string SourceType { get; set; }
        public int SourceAssetId { get; set; }
        public string TargetType { get; set; }
        public int TargetAssetId { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }
        public decimal ValueByUsd {get;set;} 
        
        public DateTime LastChanged { get; set; } = DateTime.Now;

        public Transaction(TransactionType transactionType, string description, string sourceType, int sourceAssetId,
            string targetType, int targetAssetId, decimal amount, string unit, decimal valueByUsd)
        {
            TransactionType = transactionType;
            Description = description;
            SourceType = sourceType;
            SourceAssetId = sourceAssetId;
            TargetType = targetType;
            TargetAssetId = targetAssetId;
            Amount = amount;
            Unit = unit;
            ValueByUsd = valueByUsd;
        }

        public Transaction(){}
    }
}