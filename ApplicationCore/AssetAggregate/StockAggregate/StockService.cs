using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public class StockService: IStockService
    {
        private readonly IBaseRepository<Stock> _stockRepository;
        private readonly IStockPriceRepository _stockPriceRepository;
        public StockService(IBaseRepository<Stock> stockRepository, IStockPriceRepository stockPriceRepository)
        {
            _stockRepository = stockRepository;
            _stockPriceRepository = stockPriceRepository;
        }

        public Stock CreateNewStockAsset(int portfolioId, StockDto dto)
        {
            var newStock = dto.Adapt<Stock>();
            newStock.PortfolioId = portfolioId;
            _stockRepository.Insert(newStock);
            return newStock;
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