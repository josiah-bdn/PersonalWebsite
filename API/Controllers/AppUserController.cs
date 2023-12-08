using System;
using API.Logic.Services.AppUserLogic;
using AutoMapper;
using Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {

    public class AppUserController : BaseApiController {
        private readonly IAppUserService _userService;

        public AppUserController(IAppUserService userService) {
            _userService = userService;
        }

        [Authorize]
        [HttpPost("CompleteProfile")]
        public async Task<IActionResult> CreateUserController(CompleteProfileDto user) {
            await _userService.UpdateUserProfileAsync(GetUserId(), user);
            return Ok();
        }

        [HttpGet("GetUser/{userId}")]
        public async Task<GetUserDto> GetUserController(Guid userId) {
            return await _userService.GetUserAsync(userId);
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUserController(EditUserDto editUserDto) {
            await _userService.EditUserAsync(editUserDto);
            return Ok();
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUserController(Guid userId) {
            await _userService.DeleteUserAsync(userId);
            return Ok();
        }

    }
}

