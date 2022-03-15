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


    }
}