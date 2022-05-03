using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public interface ICashService : IBaseAssetService<CashAsset>
    {
        Task<CashAsset> CreateNewCashAsset(int portfolioId, CashDto dto);

        Task<decimal> CalculateSumByPortfolio(int portfolioId, string currencyCode);
    }
}