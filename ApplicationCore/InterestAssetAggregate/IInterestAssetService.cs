using ApplicationCore.Entity.Asset;

namespace ApplicationCore.InterestAssetAggregate
{
    public interface IInterestAssetService
    {
        InterestAsset GetInterestedAssetById(int id);

        CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName);
    }
}