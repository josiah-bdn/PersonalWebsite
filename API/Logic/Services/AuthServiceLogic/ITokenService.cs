using System;
namespace API.Logic.Services.AuthServiceLogic
{
    public interface ITokenService
    {
        public string GenerateJwtToken(Guid appUserId);

        public (string HashPassword, string PasswordSalt) HashPassword(string password);

        public bool ValidatePassword(string password, string storedHash, string storedSalt);
    }
}
