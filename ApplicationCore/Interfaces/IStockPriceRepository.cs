using System;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.StockAggregate.DTOs;

namespace ApplicationCore.Interfaces
{
    public interface IStockPriceRepository
    {
        public Task<StockPriceDto> GetPriceInUsd(string symbolCode);
        public Task<decimal> GetPassPriceInUsd(string symbolCode, DateTime time);
    }
}