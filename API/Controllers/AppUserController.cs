using System;
using API.Logic.Services.AppUserLogic;
using AutoMapper;
using Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {

    public class AppUserController : BaseApiController {
        private readonly IAppUserService _userService;

        public AppUserController(IAppUserService userService) {
            _userService = userService;
        }


        [HttpPost("CreateAppUser")]
        public async Task<IActionResult> CreateUserController(CreateAppUserDto user) {
            await _userService.CreateUserAsync(user);
            return Ok();
        }

        [HttpGet("GetAppUser/{userId}")]
        public async Task<GetUserDto> GetUserController(Guid userId) {
            return await _userService.GetUserAsync(userId);
        }

        [HttpPut("EditAppUser")]
        public async Task<IActionResult> EditUserController(EditUserDto editUserDto) {
            await _userService.EditUserAsync(editUserDto);
            return Ok();
        }

        [HttpDelete("DeleteAppUser/{userId}")]
        public async Task<IActionResult> DeleteUserController(Guid userId) {
            await _userService.DeleteUserAsync(userId);
            return Ok();
        }

    }
}

