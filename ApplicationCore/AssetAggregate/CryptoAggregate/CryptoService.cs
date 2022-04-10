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
        public CryptoService(IBaseRepository<Crypto> cryptoRepository, ICryptoRateRepository cryptoRateRepository)
        {
            this._cryptoRepository = cryptoRepository;
            _cryptoRateRepository = cryptoRateRepository;
        }

    
        public Crypto CreateNewCryptoAsset(int portfolioId, CryptoDto dto)
        {
            var newCryptoAsset = new Crypto();
            dto.Adapt(newCryptoAsset);
            newCryptoAsset.PortfolioId = portfolioId; 

            _cryptoRepository.Insert(newCryptoAsset);

            return newCryptoAsset;
        }

        public async Task<List<Crypto>> GetCryptoAssetByPortfolio(int portfolioId)
        {
            var listCrypto = _cryptoRepository.List(c => c.PortfolioId == portfolioId).ToList();
            foreach (var crypto in listCrypto)
            {
                crypto.CurrentPrice = await _cryptoRateRepository.GetCurrentPrice(crypto.CryptoCoinCode, crypto.CurrencyCode);
            }
            return listCrypto.ToList(); 
        }
    }
}