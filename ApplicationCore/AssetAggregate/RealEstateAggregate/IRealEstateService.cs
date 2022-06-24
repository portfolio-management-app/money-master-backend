using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public interface IRealEstateService : IBaseAssetService<RealEstateAsset>
    {
        Task<RealEstateAsset> CreateNewRealEstateAsset(int portfolioId, RealEstateDto dto);
        RealEstateAsset UpdateRealEstateAsset(int portfolioId, int realEstateId, RealEstateDto dto);
        Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode);
    }
}