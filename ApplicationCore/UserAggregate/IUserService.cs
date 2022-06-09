using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Entity.Utilities;

namespace ApplicationCore.UserAggregate
{
    public interface IUserService
    {
        User GetUserById(int id);
        User AddNewUser(string email, string password);
        User TryAuthenticate(string email, string password);

        Task<User> TryGoogleAuthentication(string token);

        UserMobileFcmCode AddFcmCode(int userId, string newFcmCode);
    }
}