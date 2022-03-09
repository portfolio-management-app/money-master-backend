using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public class CashService : ICashService
    {
        private readonly IBaseRepository<CashAsset> _cashRepository;

        public CashService(IBaseRepository<CashAsset> cashRepository)
        {
            _cashRepository = cashRepository;
        }

        public CashAsset GetCashByUser(int userId)
        {
            return _cashRepository.GetFirst(c => c.UserId == userId);
        }

        public CashAsset EditCashValue(int userId, double newValue)
        {
            throw new System.NotImplementedException();
        }
    }
}