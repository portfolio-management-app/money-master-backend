using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CustomAssetAggregate.DTOs;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using ApplicationCore.InvestFundAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.AssetAggregate.CustomAssetAggregate
{
    public class CustomAssetService : ICustomAssetService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<CustomInterestAssetInfo> _customInterestAssetInfoRepo;
        private readonly IBaseRepository<CustomInterestAsset> _customInterestAssetRepo;
        private readonly IBaseRepository<Portfolio> _portfolioRepo;
        private readonly IInvestFundService _investFundService;
        private readonly ExternalPriceFacade _priceFacade;

        public CustomAssetService(IBaseRepository<User> userRepository,
            IBaseRepository<CustomInterestAssetInfo> customInterestAssetInfoRepo,
            IBaseRepository<CustomInterestAsset> customInterestAssetRepo, IBaseRepository<Portfolio> portfolioRepo,
             IInvestFundService investFundService, ExternalPriceFacade priceFacade)
        {
            _userRepository = userRepository;
            _customInterestAssetInfoRepo = customInterestAssetInfoRepo;
            _customInterestAssetRepo = customInterestAssetRepo;
            _portfolioRepo = portfolioRepo;
            _investFundService = investFundService;
            _priceFacade = priceFacade;
        }


        public CustomInterestAssetInfo AddCustomInterestAssetInfo(int userId, string customName)
        {
            var foundUser = _userRepository.GetFirst(user => user.Id == userId);
            if (foundUser is null)
                throw new ApplicationException("User does not exist");
            var newCustomCategory = new CustomInterestAssetInfo()
            {
                User = foundUser,
                UserId = foundUser.Id,
                Name = customName
            };

            _customInterestAssetInfoRepo.Insert(newCustomCategory);

            return newCustomCategory;
        }


        public async Task<CustomInterestAsset> AddCustomInterestAsset(int userId, int customInterestInfoId,
            int portfolioId,
            CreateNewCustomInterestAssetDto dto)
        {
            var foundPortfolio = _portfolioRepo.GetFirst(p => p.Id == portfolioId);
            if (foundPortfolio.UserId != userId)
                throw new ApplicationException("Unauthorized to access this portfolio");
            var foundCustomCategory = _customInterestAssetInfoRepo.GetFirst(c => c.Id == customInterestInfoId);
            if (foundCustomCategory.UserId != userId)
                throw new ApplicationException("Unauthorized to access this category");

            var newAsset = dto.Adapt<CustomInterestAsset>();
            newAsset.PortfolioId = portfolioId;
            _customInterestAssetRepo.Insert(newAsset);
            if (!dto.IsUsingInvestFund) return newAsset;
            var useFundResult = await _investFundService.BuyUsingInvestFund(portfolioId, newAsset);
            if (useFundResult) return newAsset;
            _customInterestAssetRepo.Delete(newAsset);
            throw new InvalidOperationException("Insufficient money amount in fund");
        }

        public List<CustomInterestAsset> GetAllCustomInterestAssetsByPortfolio(int portfolioId)
        {
            var foundCustomInterestAsset = _customInterestAssetRepo
                .List(ca => ca.PortfolioId == portfolioId, null
                    , c => c.Include(ca => ca.CustomInterestAssetInfo));

            return foundCustomInterestAsset.ToList();
        }

        public List<CustomInterestAssetInfo> GetAllUserCustomInterestAssetCategory(int userId)
        {
            var foundCategories =
                _customInterestAssetInfoRepo.List(ci => ci.UserId == userId);
            return foundCategories.ToList();
        }




        public async Task<decimal> CalculateSumCustomInterestAssetByPortfolio(int portfolioId, string currencyCode)
        {
            var customAsset = await ListByPortfolio(portfolioId);
            var unifyCurrencyValue =
                customAsset.Select(ca =>
                    ca.CalculateValueInCurrency(currencyCode, _priceFacade
                        ));
            var resultCalc = await Task.WhenAll(unifyCurrencyValue);
            var sumCash = resultCalc.Sum();
            return sumCash;
        }

        public CustomInterestAsset GetById(int assetId)
        {
            
            return _customInterestAssetRepo.GetFirst(c => c.Id == assetId);
        }

        public async  Task<List<CustomInterestAsset>> ListByPortfolio(int portfolioId)
        {
            var foundCustomInterestAsset = _customInterestAssetRepo
                .List(ca => ca.PortfolioId == portfolioId, null
                    , c => c.Include(ca => ca.CustomInterestAssetInfo));
            return foundCustomInterestAsset.ToList();
        }

        public CustomInterestAsset SetAssetToDelete(int assetId)
        {
            var found = _customInterestAssetRepo.GetFirst(c => c.Id == assetId);
            if (found is null)
                return null;
            _customInterestAssetRepo.SetToDeleted(found);
            return found;
        }
    }
}