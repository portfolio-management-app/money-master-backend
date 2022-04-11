using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;

namespace ApplicationCore.Interfaces
{
    public interface IStockPriceRepository
    {
        public Task<StockPriceDto> GetPrice(string symbolCode);
    }
}