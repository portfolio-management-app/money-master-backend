using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.StockAggregate
{
    public interface IStockService : IBaseAssetService<Stock>
    {
        Task<Stock> CreateNewStockAsset(int portfolioId, StockDto dto);
        
        
        Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode);
        
        Stock EditStock(int stockId,EditStockDto editStockDto); 
    }
}