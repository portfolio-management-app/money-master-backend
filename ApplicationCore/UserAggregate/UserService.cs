using System;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

namespace ApplicationCore.UserAggregate
{
    public class UserService: IUserService
    {
        private readonly IBaseRepository<User> _userRepository;

        public UserService(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
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
    }
}