using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Logic.Services.AuthServiceLogic
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecretKey;

        public TokenService(IConfiguration configuration) {
            _jwtSecretKey = configuration["JWT_SECRET_KEY"] ?? "defaultSecretKey";
        }

        public string GenerateJwtToken(Guid appUserId) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, appUserId.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (string HashPassword, string PasswordSalt) HashPassword(string password) {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var hashPassword = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            var passwordSalt = Convert.ToBase64String(hmac.Key);
            return (hashPassword, passwordSalt);
        }

        public bool ValidatePassword(string password, string storedHash, string storedSalt) {
            using var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(storedSalt));
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(Convert.FromBase64String(storedHash));
        }

    }
}

