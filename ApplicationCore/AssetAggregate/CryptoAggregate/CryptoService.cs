using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public class CryptoService: ICryptoService
    {
        private readonly IBaseRepository<Crypto> _cryptoRepository;
        private readonly ICryptoRateRepository _cryptoRateRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        public CryptoService(IBaseRepository<Crypto> cryptoRepository, ICryptoRateRepository cryptoRateRepository, ICurrencyRateRepository currencyRateRepository)
        {
            this._cryptoRepository = cryptoRepository;
            _cryptoRateRepository = cryptoRateRepository;
            _currencyRateRepository = currencyRateRepository;
        }

    
        public async Task<Crypto> CreateNewCryptoAsset(int portfolioId, CryptoDto dto)
        {
            var newCryptoAsset = new Crypto();
            dto.Adapt(newCryptoAsset);
            newCryptoAsset.PortfolioId = portfolioId; 

            _cryptoRepository.Insert(newCryptoAsset);
            newCryptoAsset.CurrentPrice =
                await _cryptoRateRepository.GetCurrentPriceInCurrency(newCryptoAsset.CryptoCoinCode, newCryptoAsset.CurrencyCode);

            return newCryptoAsset;
        }

        public async Task<List<Crypto>> GetCryptoAssetByPortfolio(int portfolioId)
        {
            var listCrypto = _cryptoRepository.List(c => c.PortfolioId == portfolioId).ToList();
            foreach (var crypto in listCrypto)
            {
                crypto.CurrentPrice = await _cryptoRateRepository.GetCurrentPriceInCurrency(crypto.CryptoCoinCode, crypto.CurrencyCode);
            }
            return listCrypto.ToList(); 
        }

        public async Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode)
        {
            var cryptoAssets = await GetCryptoAssetByPortfolio(portfolioId);
            var unifyCurrencyValue =
                cryptoAssets.Select(stock =>
                    stock.CalculateValueInCurrency(currencyCode, _currencyRateRepository, _cryptoRateRepository));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash; 
        }
    }
}