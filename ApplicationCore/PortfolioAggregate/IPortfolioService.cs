using System.Collections.Generic;
using ApplicationCore.Entity;

namespace ApplicationCore.PortfolioAggregate
{
    public interface IPortfolioService
    {
        public Portfolio CreatePortfolio(int userId, string name, decimal initialCash, string initialCurrency);
        public Portfolio GetPortfolioById(int portfolioId);
        public List<Portfolio> GetPortfolioList(int userId);
    }
}