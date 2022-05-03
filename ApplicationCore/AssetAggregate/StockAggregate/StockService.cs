using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using Mapster;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public class StockService : IStockService
    {
        private readonly IBaseRepository<Stock> _stockRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly IInvestFundService _investFundService;

        public StockService(IBaseRepository<Stock> stockRepository, IStockPriceRepository stockPriceRepository,
            ICryptoRateRepository cryptoRateRepository, ICurrencyRateRepository currencyRateRepository,
            IInvestFundService investFundService)
        {
            _stockRepository = stockRepository;
            _stockPriceRepository = stockPriceRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
            _investFundService = investFundService;
        }

        public async Task<Stock> CreateNewStockAsset(int portfolioId, StockDto dto)
        {
            var newAsset = dto.Adapt<Stock>();
            newAsset.PortfolioId = portfolioId;
            _stockRepository.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _stockRepository.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cashAssets = ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cashAssets
                    .Select
                    (stock =>
                        stock.CalculateValueInCurrency(currencyCode, _currencyRateRepository,
                            _cryptoRateRepository, _stockPriceRepository));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }

        public Stock GetById(int assetId)
        {
            return _stockRepository.GetFirst(s => s.Id == assetId);
        }

        public List<Stock> ListByPortfolio(int portfolioId)
        {
            var stocks = _stockRepository.List(s => s.PortfolioId == portfolioId).ToList();

            return stocks;
        }

        public Stock SetAssetToDelete(int assetId)
        {
            var found = _stockRepository.GetFirst(c => c.Id == assetId);
            if (found is null)
                return null;
            _stockRepository.SetToDeleted(found);
            return found;
        }
    }
}