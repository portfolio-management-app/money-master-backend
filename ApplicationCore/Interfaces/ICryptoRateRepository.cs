using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICryptoRateRepository
    {
        public Task<decimal> GetCurrentPrice(string cryptoId, string currencyCode);
    }
}