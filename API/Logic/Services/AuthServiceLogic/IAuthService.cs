using System;
using Data.DTO;

namespace API.Logic.Services.AuthServiceLogic
{
    public interface IAuthService
    {
        public Task<string> RegisterUserAsync(RegisterDto register);

        public Task<string> LoginAsync(LoginDto login);

        public Task SendResetEmailAsync(string email);

        public Task ValidateResetCodeAsync(ResetCodeConfirmation reset);

        public Task<string> ResetPasswordAysnc(ResetPasswordDto reset);
    }
}

