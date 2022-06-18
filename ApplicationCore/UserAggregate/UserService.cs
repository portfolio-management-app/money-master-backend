using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Utilities;
using ApplicationCore.Interfaces;
using Google.Apis.Auth;
using System.Linq;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ApplicationCore.UserAggregate
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserMobileFcmCode> _userFcmRepository;

        private readonly IBaseRepository<OTP> _otpRepository;

        public UserService(IBaseRepository<User> userRepository, IBaseRepository<UserMobileFcmCode> userFcmRepository, IBaseRepository<OTP> otpRepository)
        {
            _userRepository = userRepository;
            _userFcmRepository = userFcmRepository;
            _otpRepository = otpRepository;
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

            var existCode = _userFcmRepository.GetFirst(item => item.UserId == userId && item.FcmCode == newFcmCode);
            if (existCode is null)
            {
                var newFcm = new UserMobileFcmCode()
                {
                    FcmCode = newFcmCode,
                    UserId = userId
                };

                _userFcmRepository.Insert(newFcm);
                return newFcm;
            }
            return null;

        }
        public List<string> GetUserFcmCodeByUserId(int userId)
        {
            var result = _userFcmRepository.List((item) => item.UserId == userId).Select(u => u.FcmCode).ToList();
            return result;
        }

        public void DeleteOtpByEmail(string email)
        {
            var found = _otpRepository.GetFirst(item => item.Email == email);
            if (found is not null)
            {
                _otpRepository.Delete(found);
            }
        }

        public bool VerifyOtpCode(string email, string otpCode)
        {
            var found = _otpRepository.GetFirst(item => item.Email == email);
            if (found is not null)
            {
                var currentTime = DateTime.Now;
                var diff = currentTime - found.CreateDate;
                if (diff.TotalMinutes < 5)
                {
                    if (found.Code == otpCode)
                    {
                        return true;
                    }
                }
                else
                {
                    throw new ApplicationException("OTP verify time is over");
                }

            }
            return false;
        }
        public OTP AddNewOtp(string email, string OTP)
        {
            var existedOTP = _otpRepository.GetFirst(item => item.Email == email);
            if (existedOTP is null)
            {
                var otp = new OTP();
                otp.Code = OTP;
                otp.Email = email;
                _otpRepository.Insert(otp);
                return otp;
            }
            existedOTP.CreateDate = DateTime.Now;
            existedOTP.Code = OTP;
            _otpRepository.Update(existedOTP);
            return existedOTP;
        }

        public User SetPassword(string email, string newPassword)
        {
            var existedUser = _userRepository.GetFirst(user => user.Email == email);
            if (existedUser is null)
                throw new ApplicationException($"User with email {email} does not exist");
            existedUser.SetPassword(newPassword);
            _userRepository.Update(existedUser);
            return existedUser;
        }

        public User UpdatePassword(int userId, string newPassword, string oldPassword)
        {
            var existedUser = _userRepository.GetFirst(user => user.Id == userId);
            if (existedUser is null)
                throw new ApplicationException($"User  does not exist");
            if (!existedUser.CheckPassword(oldPassword))
            {
                throw new ApplicationException("Old password not correct");
            }
            existedUser.SetPassword(newPassword);
            _userRepository.Update(existedUser);
            return existedUser;
        }
    }
}