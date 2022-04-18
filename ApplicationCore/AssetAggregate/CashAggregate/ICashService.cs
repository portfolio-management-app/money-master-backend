using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public interface ICashService
    {
        CashAsset CreateNewCashAsset(int portfolioId, CashDto dto);
        List<CashAsset> GetCashAssetsByPortfolio(int portfolioId);

        Task<decimal>  CalculateSumByPortfolio(int portfolioId, string currencyCode);
    }
}