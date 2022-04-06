using System.Collections.Generic;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public interface IStockService
    {
        Stock CreateNewStockAsset(int portfolioId, StockDto dto);
        List<Stock> GetListStockByPortfolio(int portfolioId); 
    }
}