using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public interface ICashService
    {
        CashAsset GetCashByUser(int userId); 
    }
}