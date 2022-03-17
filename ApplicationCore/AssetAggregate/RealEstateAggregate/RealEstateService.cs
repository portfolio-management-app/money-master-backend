using System.Collections.Generic;
using System.Linq;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public class RealEstateService: IRealEstateService
    {
        private IBaseRepository<RealEstateAsset> _realEstaterepository;

        public RealEstateService(IBaseRepository<RealEstateAsset> realEstaterepository)
        {
            _realEstaterepository = realEstaterepository;
        }

        public RealEstateAsset CreateNewRealEstateAsset(int portfolioId, CreateNewRealEstateDto dto)
        {
            var newRealEstate = dto.Adapt<RealEstateAsset>();
            newRealEstate.PortfolioId = portfolioId;
            _realEstaterepository.Insert(newRealEstate);
            return newRealEstate; 
        }

        public List<RealEstateAsset> GetAllRealEstateAssetByPortfolio(int portfolioId)
        {
            return _realEstaterepository.List(realEstate => realEstate.PortfolioId == portfolioId).ToList();
        }
    }
}