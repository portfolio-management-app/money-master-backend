using System;
using ApplicationCore.Entity.Asset;
using ApplicationCore.InterestAssetAggregate.DTOs;

namespace ApplicationCore.InterestAssetAggregate
{
    public interface IInterestAssetService
    {
        InterestAsset GetInterestedAssetById(int id);
        CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName);
        CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId ,CreateNewCustomInterestAssetDto dto);
    }
}