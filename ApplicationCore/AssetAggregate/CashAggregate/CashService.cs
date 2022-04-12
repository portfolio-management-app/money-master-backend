using System.Collections.Generic;
using System.Linq;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.CashAggregate
{
    public class CashService : ICashService
    {
        private readonly IBaseRepository<CashAsset> _cashRepository;

        public CashService(IBaseRepository<CashAsset> cashRepository)
        {
            _cashRepository = cashRepository;
        }

        public CashAsset CreateNewCashAsset(int portfolioId, CashDto dto)
        {
            CashAsset newCashAsset = dto.Adapt<CashAsset>();
            newCashAsset.PortfolioId = portfolioId;

            _cashRepository.Insert(newCashAsset);
            return newCashAsset; 
        }

        public List<CashAsset> GetCashAssetsByPortfolio(int portfolioId)
        {
            return _cashRepository.List(c => c.PortfolioId == portfolioId).ToList(); 
        }
    }
}