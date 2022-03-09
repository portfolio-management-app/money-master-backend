using System;
using System.Collections.Generic;
using ApplicationCore.Entity.Asset;
using ApplicationCore.InterestAssetAggregate.DTOs;

namespace ApplicationCore.InterestAssetAggregate
{
    public interface IInterestAssetService
    {
        InterestAsset GetInterestedAssetById(int id);
        CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName);

        CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId,int portfolioId,
            CreateNewCustomInterestAssetDto dto);

        List<CustomInterestAsset> GetAllUserCustomInterestAsset(int userId, int customInterestInfoId);

        List<CustomInterestAssetInfo> GetAllUserCustomInterestAssetCategory(int userId);
    }
}