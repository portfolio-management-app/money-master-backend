using System;

namespace ApplicationCore.Entity.Transactions
{
    public class TransactionFactory
    {
        public Transaction CreateNewTransaction(SingleAssetTransactionType singleAssetTransactionType,
            string sourceType, int sourceAssetId,
            string targetType, int targetAssetId,
            decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}