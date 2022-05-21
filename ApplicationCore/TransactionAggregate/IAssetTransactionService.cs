using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;

namespace ApplicationCore.TransactionAggregate
{
    public interface IAssetTransactionService
    {
        SingleAssetTransaction AddCreateNewAssetTransaction(PersonalAsset asset, decimal moneyAmount, string currency);
        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset);  
        Task<SingleAssetTransaction> WithdrawToCash
            (PersonalAsset asset, int destinationCashId ,decimal amount, string currencyCode, bool isTransferringAll);

        decimal CalculateSubTransactionProfitLoss(IEnumerable<SingleAssetTransaction> singleAssetTransactions, string currencyCode ); 

        Task<SingleAssetTransaction> Fake();
    }
}