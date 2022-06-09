using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate
{
    public interface IBaseAssetService<TAsset> where TAsset : PersonalAsset
    {
        public TAsset GetById(int assetId);
        public Task<List<TAsset>> ListByPortfolio(int portfolioId);
        public TAsset SetAssetToDelete(int assetId);
    }
}