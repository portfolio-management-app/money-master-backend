using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;
using Mapster;
using Transaction = ApplicationCore.Entity.Transactions.Transaction;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public class RealEstateService : IRealEstateService
    {
        private readonly IBaseRepository<RealEstateAsset> _realEstateRepository;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        private TransactionFactory TransactionFactory { get; set; }

        public RealEstateService(IBaseRepository<RealEstateAsset> realEstateRepository,
            IBaseRepository<Transaction> transactionRepository, TransactionFactory transactionFactory,
            ICryptoRateRepository cryptoRateRepository, ICurrencyRateRepository currencyRateRepository,
            IStockPriceRepository stockPriceRepository)
        {
            _realEstateRepository = realEstateRepository;
            _transactionRepository = transactionRepository;
            TransactionFactory = transactionFactory;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
            _stockPriceRepository = stockPriceRepository;
        }

        public RealEstateAsset GetById(int assetId)
        {
            return _realEstateRepository.GetFirst(r => r.Id == assetId);
        }


        public RealEstateAsset CreateNewRealEstateAsset(int portfolioId, RealEstateDto dto)
        {
            var newRealEstate = dto.Adapt<RealEstateAsset>();
            newRealEstate.PortfolioId = portfolioId;
            _realEstateRepository.Insert(newRealEstate);

            // create a transaction

            var newTransaction = TransactionFactory
                .CreateNewTransaction
                (SingleAssetTransactionType.NewAsset,
                    "None",
                    newRealEstate.Id,
                    "realEstate",
                    newRealEstate.Id,
                    100
                );
            _transactionRepository.Insert(newTransaction);
            return newRealEstate;
        }

        public List<RealEstateAsset> ListByPortfolio(int portfolioId)
        {
            return _realEstateRepository.List(realEstate => realEstate.PortfolioId == portfolioId).ToList();
        }

        public RealEstateAsset UpdateRealEstateAsset(int portfolioId, int realEstateId, RealEstateDto dto)
        {
            var foundRealEstate =
                _realEstateRepository.GetFirst(r => r.Id == realEstateId && r.PortfolioId == portfolioId);
            if (foundRealEstate is null)
                return null;
            dto.Adapt(foundRealEstate);
            foundRealEstate.LastChanged = DateTime.Now;
            _realEstateRepository.Update(foundRealEstate);

            return foundRealEstate;
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cashAssets = ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cashAssets
                    .Select
                    (cash =>
                        cash.CalculateValueInCurrency(currencyCode, _currencyRateRepository,
                            _cryptoRateRepository, _stockPriceRepository));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }
    }
}