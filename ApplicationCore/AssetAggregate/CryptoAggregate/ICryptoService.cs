using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public interface ICryptoService
    {
        Task<Crypto> CreateNewCryptoAsset(int portfolioId, CryptoDto dto);
        Task<List<Crypto>> GetCryptoAssetByPortfolio(int portfolioId);
    }
}