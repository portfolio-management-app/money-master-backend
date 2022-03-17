using System.Collections.Generic;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public interface IRealEstateService
    {
        RealEstateAsset CreateNewRealEstateAsset(int portfolioId, CreateNewRealEstateDto dto);
        List<RealEstateAsset> GetAllRealEstateAssetByPortfolio(int portfolioId); 
    }
}