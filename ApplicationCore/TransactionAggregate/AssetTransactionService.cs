using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;

namespace ApplicationCore.TransactionAggregate
{
    public class AssetTransactionService: IAssetTransactionService
    {
        
        
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly IBaseRepository<SingleAssetTransaction> _transactionRepository;

        public AssetTransactionService(ICurrencyRateRepository currencyRateRepository, IStockPriceRepository stockPriceRepository, ICryptoRateRepository cryptoRateRepository, IBaseRepository<SingleAssetTransaction> transactionRepository)
        {
            _currencyRateRepository = currencyRateRepository;
            _stockPriceRepository = stockPriceRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _transactionRepository = transactionRepository;
        }

        public SingleAssetTransaction AddCreateNewAssetTransaction(PersonalAsset asset, decimal moneyAmount, string currency)
        {
            var newAssetTransaction = new SingleAssetTransaction()
            {
                SingleAssetTransactionTypes = SingleAssetTransactionTypes.NewAsset,
                SingleAssetTransactionDestination = SingleAssetTransactionDestination.Self,
                ReferentialAssetId = asset.Id,
                ReferentialAssetType = asset.GetAssetType(),
                Amount = moneyAmount,
                CreatedAt = DateTime.Now,
                CurrencyCode = currency,
                LastChanged = DateTime.Now

            };

            _transactionRepository.Insert(newAssetTransaction);
            return newAssetTransaction;

        }

        public List<SingleAssetTransaction> GetTransactionListByAsset(PersonalAsset asset)
        {
            var listTransaction = _transactionRepository.List(
                trans => trans.ReferentialAssetId == asset.Id &&
                         trans.ReferentialAssetType == asset.GetAssetType());
            return listTransaction.ToList(); 
        }
    }
}