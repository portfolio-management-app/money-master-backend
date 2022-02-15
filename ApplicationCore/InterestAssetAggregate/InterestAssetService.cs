using System;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Interfaces;

namespace ApplicationCore.InterestAssetAggregate
{
    public class InterestAssetService: IInterestAssetService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<CustomInterestAssetInfo> _customInterestAssetInfoRepo;


        public InterestAssetService(IBaseRepository<User> userRepository, IBaseRepository<CustomInterestAssetInfo> customInterestAssetInfoRepo)
        {
            _userRepository = userRepository;
            _customInterestAssetInfoRepo = customInterestAssetInfoRepo;
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
    }
}