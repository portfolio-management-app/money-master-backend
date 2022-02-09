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

        public User AddNewUser(string email, string password)
        {
            var existedUser = _userRepository.GetFirst(user => user.Email == email);
            if (existedUser is not null) throw new ApplicationException("User already existed");

            var newUser = new User(email, password);
            _userRepository.Insert(newUser);
            return newUser;
        }
    }
}