using System.Collections.Generic;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;

namespace ApplicationCore.TransactionAggregate
{
    public interface IAssetTransactionService
    {
        SingleAssetTransaction AddCreateNewAssetTransaction(PersonalAsset asset, decimal moneyAmount, string currency);
        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset);
    }
}