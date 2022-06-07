using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    public interface ICryptoRateRepository
    {
        public Task<decimal> GetCurrentPriceInCurrency(string cryptoId, string currencyCode);
        public Task<decimal> GetPastPriceInCurrency(string cryptoId, string currencyCode, DateTime dateTime);
        public Task<Dictionary<string, Dictionary<string, decimal>>> GetListCoinPrice(string coinIds, string currencies);
    }
}