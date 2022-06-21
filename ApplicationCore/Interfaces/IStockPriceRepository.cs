using System;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;

namespace ApplicationCore.Interfaces
{
    public interface IStockPriceRepository
    {
        public Task<StockPriceDto> GetPrice(string symbolCode);
        public Task<StockPriceDto> GetPassPrice(string symbolCode, DateTime time);
    }
}