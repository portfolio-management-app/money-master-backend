using System.Collections.Generic;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;

namespace ApplicationCore.PortfolioAggregate
{
    public interface IPortfolioService
    {
        public Portfolio CreatePortfolio(int userId, string name, decimal initialCash, string initialCurrency);
        public Portfolio GetPortfolioById(int portfolioId);
        public List<Portfolio> GetPortfolioList(int userId);

        public PersonalAsset GetAssetByPortfolioAndAssetId(int portfolioId, string assetType, int assetId);
    }
}