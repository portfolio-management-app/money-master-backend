using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.CustomAssetAggregate
{
    public interface ICustomAssetService: IBaseAssetService<CustomInterestAsset>
    {
        CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName);

        CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId, int portfolioId,
            CreateNewCustomInterestAssetDto dto);

        List<CustomInterestAsset> GetAllCustomInterestAssetsByPortfolio(int portfolioId);

        List<CustomInterestAssetInfo> GetAllUserCustomInterestAssetCategory(int userId);
        Task<decimal> CalculateSumCustomInterestAssetByPortfolio(int portfolioId, string currencyCode);
    }
}