using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.TransactionAggregate.DTOs;

namespace ApplicationCore.TransactionAggregate
{
    public interface IAssetTransactionService
    {
        SingleAssetTransaction AddCreateNewAssetTransaction
        (PersonalAsset asset,
            decimal moneyAmount,
            string currency,
            bool isUsingInvestFund,
            bool isUsingCash, int? usingCashId,
            decimal? fee, decimal? tax);

        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset);

        Task<SingleAssetTransaction> CreateWithdrawToCashTransaction
            (CreateTransactionDto createTransactionDto);

        Task<SingleAssetTransaction> CreateAddValueTransaction(int requestPortfolioId,
            CreateTransactionDto createTransactionDto);

        Task<SingleAssetTransaction> CreateWithdrawToOutsideTransaction(CreateTransactionDto createTransactionDto);

        decimal CalculateSubTransactionProfitLoss(IEnumerable<SingleAssetTransaction> singleAssetTransactions,
            string currencyCode);

        List<SingleAssetTransaction> GetTransactionsByType(
            params SingleAssetTransactionTypes[] assetTransactionTypesArray);

        Task<SingleAssetTransaction> Fake();
    }
}