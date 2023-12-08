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
    }
}

