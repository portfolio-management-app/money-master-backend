using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Utilities;
using System.Collections.Generic;

namespace ApplicationCore.UserAggregate
{
    public interface IUserService
    {
        User GetUserById(int id);
        User AddNewUser(string email, string password);
        public User GetUserByEmail(string email);
        User TryAuthenticate(string email, string password);

        Task<User> TryGoogleAuthentication(string token);

        UserMobileFcmCode AddFcmCode(int userId, string newFcmCode);

        public List<string> GetUserFcmCodeByUserId(int userId);

        public bool VerifyOtpCode(string email, string otpCode);
        public User SetPassword(string email, string newPassword);
        public User UpdatePassword(int userId, string newPassword, string oldPassword);
        public OTP AddNewOtp(string email, string OTP);

    }
}