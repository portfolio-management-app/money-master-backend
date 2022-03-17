using System.Collections.Generic;
using ApplicationCore.Entity;

namespace ApplicationCore.PortfolioAggregate
{
    public interface IPortfolioService
    {
        public Portfolio CreatePortfolio(int userId, string name, double initialCash, string initialCurrency);
        public List<Portfolio> GetPortfolioList(int userId); 
    }
}