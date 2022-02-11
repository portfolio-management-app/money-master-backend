using ApplicationCore.Entity;

namespace ApplicationCore.UserAggregate
{
    public interface IUserService
    {
        User GetUserById(int id);
        User GetUserByEmail(string email);
        User AddNewUser(string email, string password);
        User TryAuthenticate(string email, string password);
    }
}