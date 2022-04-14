using System.Collections.Generic;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.InterestAssetAggregate
{
    public interface IInterestAssetService
    {
        CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName);

        CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId, int portfolioId,
            CreateNewCustomInterestAssetDto dto);

        List<CustomInterestAsset> GetAllUserCustomInterestAssetInCategory(int userId, int customInterestInfoId);
        List<CustomInterestAsset> GetALlCustomInterestAssets(int portfolioId); 

        List<CustomInterestAssetInfo> GetAllUserCustomInterestAssetCategory(int userId);
        BankSavingAsset AddBankSavingAsset(int portfolioId, CreateNewBankSavingAssetDto commandDto);
        List<BankSavingAsset> GetAllPortfolioBankSavingAssets(int portfolioId);
        BankSavingAsset EditBankSavingAsset(int portfolioId, int bankingAssetId, EditBankSavingAssetDto dto);
    }
}