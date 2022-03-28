using System.Collections.Generic;
using System.Linq;
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

        public Portfolio CreatePortfolio(int userId, string name, decimal initialCash, string initialCurrency)
        {
            var newPortfolio = new Portfolio(userId, name, initialCash, initialCurrency);
            _portfolioRepository.Insert(newPortfolio);
            return newPortfolio;
        }

        public Portfolio GetPortfolioById(int portfolioId)
        {
            var foundPortfolio = _portfolioRepository.GetFirst(p => p.Id == portfolioId);
            return foundPortfolio;
        }

        public List<Portfolio> GetPortfolioList(int userId)
        {
            var listPortfolio = _portfolioRepository.List(p => p.UserId == userId).ToList();
            return listPortfolio;
        }
    }
}