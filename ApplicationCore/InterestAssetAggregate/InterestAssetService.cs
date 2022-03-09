using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.InterestAssetAggregate.DTOs;
using ApplicationCore.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.InterestAssetAggregate
{
    public class InterestAssetService : IInterestAssetService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<CustomInterestAssetInfo> _customInterestAssetInfoRepo;
        private readonly IBaseRepository<CustomInterestAsset> _customInterestAssetRepo;
        private readonly IBaseRepository<Portfolio> _portfolioRepo;

        public InterestAssetService(IBaseRepository<User> userRepository,
            IBaseRepository<CustomInterestAssetInfo> customInterestAssetInfoRepo,
            IBaseRepository<CustomInterestAsset> customInterestAssetRepo, IBaseRepository<Portfolio> portfolioRepo)
        {
            _userRepository = userRepository;
            _customInterestAssetInfoRepo = customInterestAssetInfoRepo;
            _customInterestAssetRepo = customInterestAssetRepo;
            _portfolioRepo = portfolioRepo;
        }

        public InterestAsset GetInterestedAssetById(int id)
        {
            throw new NotImplementedException();
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



        public CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId, int portfolioId,
            CreateNewCustomInterestAssetDto dto)
        {

            var foundPortfolio = _portfolioRepo.GetFirst(p => p.Id == portfolioId);
            if (foundPortfolio.UserId != userId)
                throw new ApplicationException("Unauthorized to access this portfolio"); 
            var foundCustomCategory = _customInterestAssetInfoRepo.GetFirst(c => c.Id == customInterestInfoId);
            if (foundCustomCategory.UserId!= userId)
                throw new ApplicationException("Unauthorized to access this category");

            var newCustomInterestAsset = dto.Adapt<CustomInterestAsset>();
            newCustomInterestAsset.UserId = userId;
            newCustomInterestAsset.CustomInterestAssetInfo = foundCustomCategory;
            newCustomInterestAsset.Portfolio = foundPortfolio;
            _customInterestAssetRepo.Insert(newCustomInterestAsset);
            return newCustomInterestAsset;
        }

        public List<CustomInterestAsset> GetAllUserCustomInterestAsset(int userId, int customInterestInfoId)
        {
            var foundCategory =
                _customInterestAssetInfoRepo.GetFirst(ci => ci.Id == customInterestInfoId);
            if (foundCategory is null)
                throw new ApplicationException("Unauthorized access for this category");

            if (foundCategory.UserId != userId)
                throw new ApplicationException("Unauthorized access for this category");
            var list = _customInterestAssetRepo
                .List(c => c.CustomInterestAssetInfoId == foundCategory.Id);
            return list.ToList();
        }

        public List<CustomInterestAssetInfo> GetAllUserCustomInterestAssetCategory(int userId)
        {
            var foundCategories =
                _customInterestAssetInfoRepo.List(ci => ci.UserId == userId);
            return foundCategories.ToList();
        }
    }
}