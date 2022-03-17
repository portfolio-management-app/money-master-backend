using System.Collections.Generic;
using ApplicationCore.Entity.Asset;
using ApplicationCore.InterestAssetAggregate.DTOs;

namespace ApplicationCore.AssetAggregate.InterestAssetAggregate
{
    public interface IInterestAssetService
    {
        InterestAsset GetInterestedAssetById(int id);
        CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName);

        CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId,int portfolioId,
            CreateNewCustomInterestAssetDto dto);

        List<CustomInterestAsset> GetAllUserCustomInterestAsset(int userId, int customInterestInfoId);

        List<CustomInterestAssetInfo> GetAllUserCustomInterestAssetCategory(int userId);
        BankSavingAsset AddBankSavingAsset(int userId, int portfolioId, CreateNewBankSavingAssetDto commandDto);
        List<BankSavingAsset> GetAllPortfolioBankSavingAssets(int portfolioId); 
    }
}