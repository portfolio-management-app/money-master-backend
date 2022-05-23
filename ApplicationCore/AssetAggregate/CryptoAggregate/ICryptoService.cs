using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CryptoAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;

namespace ApplicationCore.AssetAggregate.CryptoAggregate
{
    public interface ICryptoService : IBaseAssetService<Crypto>
    {
        Task<Crypto> CreateNewCryptoAsset(int portfolioId, CryptoDto dto);
        Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode);

        Crypto EditCrypto(int cryptoId, EditCryptoDto dto);
    }
}