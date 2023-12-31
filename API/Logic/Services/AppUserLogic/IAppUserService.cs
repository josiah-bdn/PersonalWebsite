﻿using System;
using Data.DTO;

namespace API.Logic.Services.AppUserLogic
{
    public interface IAppUserService {

        public Task UpdateUserProfileAsync(Guid userId, CompleteProfileDto appUser);

        public Task<GetUserDto> GetUserAsync(Guid guid);

        public Task DeleteUserAsync(Guid userId);

        public Task EditUserAsync(EditUserDto editUserDto);
    };
}

