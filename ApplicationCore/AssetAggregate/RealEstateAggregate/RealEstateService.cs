using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.AssetAggregate.RealEstateAggregate.DTOs;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.RealEstateAggregate
{
    public class RealEstateService : IRealEstateService
    {
        private IBaseRepository<RealEstateAsset> _realEstaterepository;

        public RealEstateService(IBaseRepository<RealEstateAsset> realEstaterepository)
        {
            _realEstaterepository = realEstaterepository;
        }

        public RealEstateAsset CreateNewRealEstateAsset(int portfolioId, RealEstateDto dto)
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

        public RealEstateAsset UpdateRealEstateAsset(int portfolioId, int realEstateId, RealEstateDto dto)
        {
            var foundRealEstate =
                _realEstaterepository.GetFirst(r => r.Id == realEstateId && r.PortfolioId == portfolioId);
            if (foundRealEstate is null)
                return null;
            dto.Adapt(foundRealEstate);
            foundRealEstate.LastChanged = DateTime.Now;
            _realEstaterepository.Update(foundRealEstate);

            return foundRealEstate;
        }
    }
}