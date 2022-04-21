using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public class StockService : IStockService
    {
        private readonly IBaseRepository<Stock> _stockRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;

        public StockService(IBaseRepository<Stock> stockRepository, IStockPriceRepository stockPriceRepository, ICryptoRateRepository cryptoRateRepository, ICurrencyRateRepository currencyRateRepository)
        {
            _stockRepository = stockRepository;
            _stockPriceRepository = stockPriceRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
        }

        public Stock CreateNewStockAsset(int portfolioId, StockDto dto)
        {
            var newStock = dto.Adapt<Stock>();
            newStock.PortfolioId = portfolioId;
            _stockRepository.Insert(newStock);
            return newStock;
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            
            var cashAssets = ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cashAssets
                    .Select
                    (stock =>
                        stock.CalculateValueInCurrency(currencyCode, _currencyRateRepository,
                            _cryptoRateRepository,_stockPriceRepository));
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
    }
}