using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using ApplicationCore.TransactionAggregate;
using Mapster;
using Transaction = ApplicationCore.Entity.Transactions.Transaction;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public class RealEstateService : IRealEstateService
    {
        private readonly IBaseRepository<RealEstateAsset> _realEstateRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        private readonly IInvestFundService _investFundService;
        private TransactionFactory TransactionFactory { get; set; }

        public RealEstateService(IBaseRepository<RealEstateAsset> realEstateRepository,
             TransactionFactory transactionFactory,
            ICryptoRateRepository cryptoRateRepository, ICurrencyRateRepository currencyRateRepository,
            IStockPriceRepository stockPriceRepository, IInvestFundService investFundService)
        {
            _realEstateRepository = realEstateRepository;
            TransactionFactory = transactionFactory;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
            _stockPriceRepository = stockPriceRepository;
            _investFundService = investFundService;
        }

        public RealEstateAsset GetById(int assetId)
        {
            return _realEstateRepository.GetFirst(r => r.Id == assetId);
        }


        public async Task<RealEstateAsset> CreateNewRealEstateAsset(int portfolioId, RealEstateDto dto)
        {
            var newAsset = dto.Adapt<RealEstateAsset>();
            newAsset.PortfolioId = portfolioId; 
            _realEstateRepository.Insert(newAsset); 
            if (!dto.IsUsingInvestFund) return newAsset; 
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset); 
            if (useFundResult) return newAsset; 
            _realEstateRepository.Delete(newAsset); 
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public List<RealEstateAsset> ListByPortfolio(int portfolioId)
        {
            return _realEstateRepository.List(realEstate => realEstate.PortfolioId == portfolioId).ToList();
        }

        public RealEstateAsset SetAssetToDelete(int assetId)
        {
            
            var found = _realEstateRepository.GetFirst(c => c.Id == assetId);
            if (found is null)
                return null;
            _realEstateRepository.SetToDeleted(found); 
            return found; 
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