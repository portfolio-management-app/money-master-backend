using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.AssetAggregate.InterestAssetAggregate.DTOs;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;
using Mapster;

namespace ApplicationCore.AssetAggregate.InterestAssetAggregate
{
    public class InterestAssetService : IInterestAssetService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<CustomInterestAssetInfo> _customInterestAssetInfoRepo;
        private readonly IBaseRepository<CustomInterestAsset> _customInterestAssetRepo;
        private readonly IBaseRepository<Portfolio> _portfolioRepo;
        private readonly IBaseRepository<BankSavingAsset> _bankSavingRepository;

        public InterestAssetService(IBaseRepository<User> userRepository,
            IBaseRepository<CustomInterestAssetInfo> customInterestAssetInfoRepo,
            IBaseRepository<CustomInterestAsset> customInterestAssetRepo, IBaseRepository<Portfolio> portfolioRepo,
            IBaseRepository<BankSavingAsset> bankSavingRepository)
        {
            _userRepository = userRepository;
            _customInterestAssetInfoRepo = customInterestAssetInfoRepo;
            _customInterestAssetRepo = customInterestAssetRepo;
            _portfolioRepo = portfolioRepo;
            _bankSavingRepository = bankSavingRepository;
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
            if (foundCustomCategory.UserId != userId)
                throw new ApplicationException("Unauthorized to access this category");

            var newCustomInterestAsset = dto.Adapt<CustomInterestAsset>();
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

        public BankSavingAsset AddBankSavingAsset( int portfolioId, CreateNewBankSavingAssetDto commandDto)
        {
            BankSavingAsset newBankSavingAsset = commandDto.Adapt<BankSavingAsset>();
            newBankSavingAsset.LastChanged = DateTime.Now;
            newBankSavingAsset.PortfolioId = portfolioId;
            _bankSavingRepository.Insert(newBankSavingAsset);
            return newBankSavingAsset;
        }

        public List<BankSavingAsset> GetAllPortfolioBankSavingAssets(int portfolioId)
        {
            var listBankSavingAsset = _bankSavingRepository
                .List(b => b.PortfolioId == portfolioId)
                .ToList();
            return listBankSavingAsset;
        }

        public BankSavingAsset EditBankSavingAsset(int portfolioId, int bankingAssetId, EditBankSavingAssetDto dto)
        {
            var foundBankingAsset =
                _bankSavingRepository
                    .GetFirst(b => b.Id == bankingAssetId
                                   && b.PortfolioId == portfolioId);
            if (foundBankingAsset is null)
                return null;
            foundBankingAsset
                .Update(dto.Name,
                    dto.InputDay,
                    dto.InputMoneyAmount,
                    dto.InputCurrency,
                    dto.Description,
                    dto.InterestRate,
                    dto.TermRange,
                    dto.BankCode,
                    dto.IsGoingToReinState);
            
            // TODO: deal with change interest rate (continue with the current amount or reset from start)

            _bankSavingRepository.Update(foundBankingAsset);
            return foundBankingAsset;
        }
    }
}