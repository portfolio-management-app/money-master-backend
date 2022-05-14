using System.Threading.Tasks;
using ApplicationCore.Entity;

namespace ApplicationCore.UserAggregate
{
    public interface IUserService
    {
        User GetUserById(int id);
        User AddNewUser(string email, string password);
        User TryAuthenticate(string email, string password);

        Task<User> TryGoogleAuthentication(string token); 
    }
}