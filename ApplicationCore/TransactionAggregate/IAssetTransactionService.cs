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
        (int portfolioId, PersonalAsset asset,
            decimal moneyAmount,
            string currency,
            bool isUsingInvestFund,
            bool isUsingCash, int? usingCashId,
            decimal? fee, decimal? tax);

        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset, int? pageNumber, int? pageSize,
            DateTime? startDate, DateTime? endDate, string transactionType);

        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset);

        List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset, DateTime? startTime,
            DateTime? endTime);

        Task<SingleAssetTransaction> CreateWithdrawToCashTransaction
            (int portfolioId, CreateTransactionDto createTransactionDto);

        Task<SingleAssetTransaction> CreateAddValueTransaction(int portfolioId,
            CreateTransactionDto createTransactionDto);

        Task<SingleAssetTransaction> CreateWithdrawToOutsideTransaction(int portfolioId,
            CreateTransactionDto createTransactionDto);

        Task<SingleAssetTransaction> CreateMoveToFundTransaction(
            int portfolioId, CreateTransactionDto createTransactionDto);

        Task<decimal> CalculateSubTransactionProfitLoss(IEnumerable<SingleAssetTransaction> singleAssetTransactions,
            string currencyCode);

        public List<SingleAssetTransaction> GetTransactionsByType(
            int portfolioId, params SingleAssetTransactionType[] assetTransactionTypesArray);
        
        public List<SingleAssetTransaction> GetTransactionsByType(
            int portfolioId, DateTime? startTime, DateTime? endTime, params SingleAssetTransactionType[] assetTransactionTypesArray);
    }
}