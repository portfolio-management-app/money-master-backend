using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public class CryptoService: ICryptoService
    {
        private readonly IBaseRepository<Crypto> _cryptoRepository; 
        public CryptoService(IBaseRepository<Crypto> cryptoRepository)
        {
            this._cryptoRepository = cryptoRepository;
        }

    
        public Crypto CreateNewCryptoAsset(int portfolioId, CryptoDto dto)
        {
            var newCryptoAsset = new Crypto();
            dto.Adapt(newCryptoAsset);
            newCryptoAsset.PortfolioId = portfolioId; 

            _cryptoRepository.Insert(newCryptoAsset);

            return newCryptoAsset;
        }
    }
}