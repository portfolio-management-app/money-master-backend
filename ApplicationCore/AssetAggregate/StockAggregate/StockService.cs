using System.Collections.Generic;
using System.Linq;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public class StockService: IStockService
    {
        private readonly IBaseRepository<Stock> _stockRepository;

        public StockService(IBaseRepository<Stock> stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public Stock CreateNewStockAsset(int portfolioId, StockDto dto)
        {
            var newStock = dto.Adapt<Stock>();
            newStock.PortfolioId = portfolioId;
            _stockRepository.Insert(newStock);
            return newStock;
        }

        public List<Stock> GetListStockByPortfolio(int portfolioId)
        {
            return _stockRepository.List(s => s.PortfolioId == portfolioId).ToList();
        }
    }
}