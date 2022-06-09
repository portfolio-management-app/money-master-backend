using System;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICryptoRateRepository
    {
        public Task<decimal> GetCurrentPriceInCurrency(string cryptoId, string currencyCode);
        public Task<decimal> GetPastPriceInCurrency(string cryptoId, string currencyCode, DateTime dateTime);
    }
}