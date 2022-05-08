using ApplicationCore.Interfaces;

namespace ApplicationCore
{
    public class ExternalPriceFacade
    {
       public ICryptoRateRepository CryptoRateRepository { get; }
       public IStockPriceRepository StockPriceRepository { get; }
       public ICurrencyRateRepository CurrencyRateRepository { get; }

       public ExternalPriceFacade(ICryptoRateRepository cryptoRateRepository, IStockPriceRepository stockPriceRepository, ICurrencyRateRepository currencyRateRepository)
       {
           CryptoRateRepository = cryptoRateRepository;
           StockPriceRepository = stockPriceRepository;
           CurrencyRateRepository = currencyRateRepository;
       }
       
       
    }
}