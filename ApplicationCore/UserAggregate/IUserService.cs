using ApplicationCore.Entity;

namespace ApplicationCore.UserAggregate
{
    public interface IUserService
    {
        User AddNewUser(string email, string password); 
    }
}