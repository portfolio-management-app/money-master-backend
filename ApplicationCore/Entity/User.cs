using System.Data.Common;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationCore.Entity
{
    public class User: BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public User(string email, string password)
        {
            Email = email;
            SetPassword(password);
        }
        
        private User(){}

        public void SetPassword(string passwordString)
        {
            using (var randomHash = new HMACSHA512())
            {
                PasswordSalt = randomHash.Key;
                PasswordHash = randomHash.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
            }
        }
    }
}