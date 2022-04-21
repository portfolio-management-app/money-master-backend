using System.Collections.Generic;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate
{
    public interface IBaseAssetService<TAsset> where TAsset: PersonalAsset
    {
        public TAsset GetById(int assetId);
        public List<TAsset> ListByPortfolio(int portfolioId);
    }
}