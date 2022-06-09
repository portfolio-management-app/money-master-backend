using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Utilities;
using ApplicationCore.Interfaces;
using Google.Apis.Auth;

namespace ApplicationCore.UserAggregate
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserMobileFcmCode> _userFcmRepository;

        public UserService(IBaseRepository<User> userRepository, IBaseRepository<UserMobileFcmCode> userFcmRepository)
        {
            _userRepository = userRepository;
            _userFcmRepository = userFcmRepository;
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetFirst(u => u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetFirst(u => u.Email == email);
        }

        public User AddNewUser(string email, string password)
        {
            var existedUser = _userRepository.GetFirst(user => user.Email == email);
            if (existedUser is not null) throw new ApplicationException("User already existed");

            var newUser = new User(email, password);
            _userRepository.Insert(newUser);
            return newUser;
        }

        public User TryAuthenticate(string email, string password)
        {
            var existedUser = _userRepository.GetFirst(user => user.Email == email);
            if (existedUser is null)
                throw new ApplicationException($"User with email {email} does not exist");
            return existedUser.CheckPassword(password) ? existedUser : null;
        }

        public async Task<User> TryGoogleAuthentication(string token)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>
                        { "511417762868-an9ak0crrtra3c4l0rqebt5bmuuo5aqp.apps.googleusercontent.com" }
                };

                var payloadResult = await GoogleJsonWebSignature.ValidateAsync(token, settings);
                var userEmailFromGoogle = payloadResult.Email;

                var existedUser = _userRepository.GetFirst(user => user.Email == userEmailFromGoogle);
                if (existedUser is not null) return existedUser;
                var newUser = AddNewUser(userEmailFromGoogle, "defaultPassword");
                return newUser;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to login with Google: {ex.Message}");
            }
        }

        public UserMobileFcmCode AddFcmCode(int userId, string newFcmCode)
        {
            var user = GetUserById(userId);
            if (user is null)
                return null;
            var newFcm = new UserMobileFcmCode()
            {
                FcmCode = newFcmCode,
                UserId = userId
            };

            _userFcmRepository.Insert(newFcm);
            return newFcm;
        }
    }
}