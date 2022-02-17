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
    public class InterestAssetService: IInterestAssetService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<CustomInterestAssetInfo> _customInterestAssetInfoRepo;
        private readonly IBaseRepository<CustomInterestAsset> _customInterestAssetRepo;

        public InterestAssetService(IBaseRepository<User> userRepository, IBaseRepository<CustomInterestAssetInfo> customInterestAssetInfoRepo, IBaseRepository<CustomInterestAsset> customInterestAssetRepo)
        {
            _userRepository = userRepository;
            _customInterestAssetInfoRepo = customInterestAssetInfoRepo;
            _customInterestAssetRepo = customInterestAssetRepo;
        }

        public InterestAsset GetInterestedAssetById(int id)
        {
            throw new System.NotImplementedException();
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

        public CustomInterestAsset AddCustomInterestAsset(int userId, int customInterestInfoId, string name, DateTime inputDay,
            double inputMoneyAmount, string inputCurrency, string description, double interestRate, int termRange)
        {
            // check is user master of this category
            var foundCustomCategory = _customInterestAssetInfoRepo.GetFirst(c => c.Id == customInterestInfoId);
            if (foundCustomCategory.Id != userId)
                throw new ApplicationException("Unauthorized to access this category");
            var newCustomInterestAsset = new CustomInterestAsset()
            {
                UserId = userId,
                CustomInterestAssetInfo = foundCustomCategory,
                Description = description,
                InputCurrency = inputCurrency,
                InputDay = inputDay,
                InputMoneyAmount = inputMoneyAmount,
                InterestRate = interestRate,
                LastChanged = DateTime.Now,
                TermRange = termRange

            };

            _customInterestAssetRepo.Insert(newCustomInterestAsset);
            return newCustomInterestAsset;
        }

        public CustomInterestAsset AddCustomInterestAsset(int userId,int customInterestInfoId,CreateNewCustomInterestAssetDto dto)
        {
            var foundCustomCategory = _customInterestAssetInfoRepo.GetFirst(c => c.Id == customInterestInfoId);
            if (foundCustomCategory.UserId != userId)
                throw new ApplicationException("Unauthorized to access this category");

            var newCustomInterestAsset = dto.Adapt<CustomInterestAsset>();
            newCustomInterestAsset.UserId = userId;
            newCustomInterestAsset.CustomInterestAssetInfo = foundCustomCategory;
            _customInterestAssetRepo.Insert(newCustomInterestAsset);
            return newCustomInterestAsset;
        }

        public List<CustomInterestAsset> GetAllUserCustomInterestAsset(int userId, int customInterestInfoId)
        {
            var foundCategory = _customInterestAssetInfoRepo.GetFirst(ci => ci.UserId == userId);
            if (foundCategory is null)
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