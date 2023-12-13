using System;
using API.Logic.Services.AuthServiceLogic;
using Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("RegisterUser")]
        public async Task<string> RegisterUerController(RegisterDto register) {
           return await _authService.RegisterUserAsync(register);
        }

        [HttpPost("LoginUser")]
        public async Task<string> LoginUserController(LoginDto login) {
            return await _authService.LoginAsync(login);        
        }

        [HttpPost("ResetPasswordRequest")]
        public async Task SendResetEmailController(string email) {
           await _authService.SendResetEmailAsync(email);
        }

        [HttpPost("ValidateResetRequest")]
        public async Task ValidateResetReqeustController(ResetCodeConfirmation resetRequest) {
            await _authService.ValidateResetCodeAsync(resetRequest);
        }

        [HttpPost("ResetPassword")]
        public async Task<string> ResetPasswordController(ResetPasswordDto reset) {
            return await _authService.ResetPasswordAysnc(reset);
        }
    }
}

