using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICurrencyRateRepository
    {
        Task<decimal> Exchange(string sourceCurrency, string destinationCurrency); 
    }
}