using ApplicationCore.Entity;

namespace ApplicationCore.PortfolioAggregate
{
    public interface IPortfolioService
    {
        public Portfolio CreatePortfolio(int userId, string name, double initialCash, string initialCurrency);
    }
}