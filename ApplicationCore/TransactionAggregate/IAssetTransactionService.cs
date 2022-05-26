using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;

namespace ApplicationCore.TransactionAggregate
{
    public interface IAssetTransactionService
    {
        SingleAssetTransaction AddCreateNewAssetTransaction(PersonalAsset asset, decimal moneyAmount, string currency,bool isUsingInvestFund);
        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset);  
        Task<SingleAssetTransaction> CreateWithdrawToCashTransaction
            (PersonalAsset asset, int destinationCashId ,decimal amount, string currencyCode, bool isTransferringAll);

        Task<SingleAssetTransaction> CreateAddValueTransaction(PersonalAsset asset, decimal amountInAssetUnit,
            decimal? valueInCurrency, string currency);
        decimal CalculateSubTransactionProfitLoss(IEnumerable<SingleAssetTransaction> singleAssetTransactions, string currencyCode ); 

        Task<SingleAssetTransaction> Fake();
    }
}