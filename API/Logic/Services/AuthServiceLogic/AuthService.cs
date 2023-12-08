using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Data.DTO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace API.Logic.Services.AuthServiceLogic {
    public class AuthService : IAuthService {
        private readonly DataContext _db;

        public AuthService(DataContext db) {
            _db = db;
        }

        // TEST REMOVE LATER //

        public async Task<string> RegisterUserAsync(RegisterDto register) {

            if (register.Password != register.ConfirmPassword) throw new ArgumentException("Passwords do no mathc");

            if (await _db.AppUser.AnyAsync(u => u.Email == register.Email || u.UserName == register.UserName)) throw new ArgumentException("Email and or UserName already used");

            if (!IsValidEmail(register.Email)) throw new ArgumentException("Please provide a valid email");

            if (!IsValidUsername(register.UserName)) throw new ArgumentException("Please provide username that conforms to requirement");

            if (!IsValidPassword(register.Password)) throw new ArgumentException("Please provide a password that meets form requirements");

            var user = new AppUser {
                AppUserId = Guid.NewGuid(),
                Email = register.Email,
                UserName = register.UserName,
                CreatedDate = DateTime.UtcNow
            };

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var authentication = new Authentication {
                AppUserId = user.AppUserId,
                HashPassword = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(register.Password))),
                PasswordSalt = Convert.ToBase64String(hmac.Key),
                LastLogin = DateTime.UtcNow,
                LoginCount = 0
            };

            await _db.AppUser.AddAsync(user);
            await _db.Authentication.AddAsync(authentication);
            await _db.SaveChangesAsync();

            var login = new LoginDto { Email = register.Email, Password = register.Password };
            var token = await LoginAsync(login);

            return token;

        }

        public async Task<string> LoginAsync(LoginDto login) {
            var user = await _db.AppUser.Where(u => u.Email == login.Email).FirstOrDefaultAsync();

            if (user is null) {
                throw new ArgumentException("Invalid email");
            }

            var auth = await _db.Authentication.FirstOrDefaultAsync(a => a.AppUserId == user.AppUserId);

            if (auth == null) {
                throw new ArgumentException("Authentication data not found.");
            }

            using var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(auth.PasswordSalt));
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(login.Password));

            if (!computedHash.SequenceEqual(Convert.FromBase64String(auth.HashPassword))) {
                throw new ArgumentException("Invalid password.");
            }

            auth.LoginCount += 1;
            _db.Authentication.Update(auth);
            await _db.SaveChangesAsync();

            return GenerateJwtToken(user.AppUserId);
        }

        private string GenerateJwtToken(Guid appUserId) {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "defaultSecretKey");

            string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            key = Encoding.ASCII.GetBytes(secretKey);

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

        private bool IsValidEmail(string email) {
            if (string.IsNullOrEmpty(email)) {
                return false;
            }

            var emailPattern = @"^\S+@\S+\.\S+$";

            var multipleDotsPaterrn = @"(\.\.)|(@\.)|(\.@)";

            return Regex.IsMatch(email, emailPattern) && !Regex.IsMatch(email, multipleDotsPaterrn);
        }

        private bool IsValidUsername(string username) {
            if (string.IsNullOrWhiteSpace(username)) {
                return false;
            }

            int minLength = 5;
            int maxLength = 10;

            var allowedUsernamePattern = @"^[a-zA-Z0-9$!@$]{5,20}%";

            if (username.Length > minLength || username.Length < maxLength) {
                return false;
            }

            return Regex.IsMatch(username, allowedUsernamePattern);
        }

        private bool IsValidPassword(string password) {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            int minLength = 8;
            int maxLength = 15;

            var hasNumberPattern = @"[0-9]+";
            var hasSpecialCharPattern = @"[$@!#%^&*]+";

            if (password.Length < minLength || password.Length > maxLength)
                return false;

            bool hasNumber = Regex.IsMatch(password, hasNumberPattern);
            bool hasSpecialChar = Regex.IsMatch(password, hasSpecialCharPattern);

            return hasNumber && hasSpecialChar;
        }
    }

}
