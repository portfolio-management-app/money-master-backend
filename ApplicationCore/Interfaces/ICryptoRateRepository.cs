using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICryptoRateRepository
    {
        public Task<decimal> GetCurrentPriceInCurrency(string cryptoId, string currencyCode);
    }
}