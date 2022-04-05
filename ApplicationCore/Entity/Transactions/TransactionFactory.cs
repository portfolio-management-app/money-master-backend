using System;

namespace ApplicationCore.Entity.Transactions
{
    public class TransactionFactory
    {
        public Transaction CreateNewTransaction(TransactionType transactionType, string description,
            string sourceType, int sourceAssetId,
            string targetType, int targetAssetId,
            decimal amount, string unit,
            decimal valueByUsd)
        {
            var newTemporaryTransaction = new Transaction(transactionType, description, sourceType, sourceAssetId,
                targetType, targetAssetId, amount, unit, valueByUsd);

            if (!ValidateNotTheSameAsset(newTemporaryTransaction))
                throw new ApplicationException("Cannot move to the same asset");

            return newTemporaryTransaction;

        }

        private bool ValidateNotTheSameAsset(Transaction transaction)
            => transaction.SourceType != transaction.TargetType
               || transaction.SourceAssetId != transaction.TargetAssetId; 
        
    }
}