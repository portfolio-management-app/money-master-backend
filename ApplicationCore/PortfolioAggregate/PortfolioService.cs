using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

namespace ApplicationCore.PortfolioAggregate
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IBaseRepository<Portfolio> _portfolioRepository;

        public PortfolioService(IBaseRepository<Portfolio> portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public Portfolio CreatePortfolio(int userId, string name, double initialCash, string initialCurrency)
        {
            var newPortfolio = new Portfolio(userId, name, initialCash, initialCurrency);
            _portfolioRepository.Insert(newPortfolio);
            return newPortfolio;
        }
    }
}