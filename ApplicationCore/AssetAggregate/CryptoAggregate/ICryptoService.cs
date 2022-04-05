using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public interface ICryptoService
    {
        Crypto CreateNewCryptoAsset(int portfolioId, CryptoDto dto); 
    }
}