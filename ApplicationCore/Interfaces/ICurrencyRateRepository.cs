using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICurrencyRateRepository
    {
        Task<CurrencyRate> GetRateObject(string sourceCurrency);
    }
}