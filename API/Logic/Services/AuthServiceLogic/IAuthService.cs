﻿using System;
using Data.DTO;

namespace API.Logic.Services.AuthServiceLogic
{
    public interface IAuthService
    {
        public Task<string> RegisterUserAsync(RegisterDto register);

        public Task<string> LoginAsync(LoginDto login);
    }
}

