using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApplicationCore.Entity.Asset;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationCore.Entity
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
        // relationships to others table 
        public List<RealEstateAsset> RealEstateAssets { get; set; } = new List<RealEstateAsset>();
        public CashAsset CashAsset { get; set; } = new CashAsset();
        public List<InterestAsset> InterestAssets = new List<InterestAsset>();
        public List<Portfolio> Portfolios = new List<Portfolio>();

        public User(string email, string password)
        {
            Email = email;
            SetPassword(password);
        }

        private User()
        {
        }

        public void SetPassword(string passwordString)
        {
            using (var randomHash = new HMACSHA512())
            {
                PasswordSalt = randomHash.Key;
                PasswordHash = randomHash.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
            }
        }

        public bool CheckPassword(string passwordString)
        {
            if (string.IsNullOrEmpty(passwordString))
                return false;
            using var hmacsha512 = new HMACSHA512(PasswordSalt);
            var hashToCheck = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
            return !PasswordHash.Where((byt, i) => byt != hashToCheck[i]).Any();
        }

        public string GenerateToken(string signingKey)
        {
            var hourToRefresh = 48;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID",Id.ToString()),
                    new Claim(ClaimTypes.Email, Email)
                }),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddHours(hourToRefresh)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}