using System;
using Data.DTO;
using System.Linq;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using AutoMapper;

namespace API.Logic.Services.AppUserLogic {
    public class AppUserService : IAppUserService {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public AppUserService(DataContext db, IMapper mapper) {
            _db = db;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateAppUserDto appUser) {
            var user = new AppUser {
                AppUserId = Guid.NewGuid(),
                Email = appUser.Email,
                UserName = appUser.UserName,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };

            _db.Add(user);
            await _db.SaveChangesAsync();

        }

        public async Task<GetUserDto> GetUserAsync(Guid userId) {
            var user = await _db.AppUser.Where(a => a.AppUserId == userId).Select(a => new GetUserDto {
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                UserName = a.UserName
            }).FirstOrDefaultAsync();

            if (user is null) {
                throw new Exception();
            }
            return user;
        }

        public async Task DeleteUserAsync(Guid userId) {
            var user = await _db.AppUser.Where(a => a.AppUserId == userId).FirstOrDefaultAsync() ?? throw new Exception();

            _db.Remove(user);

            await _db.SaveChangesAsync();
        }

        public async Task EditUserAsync(EditUserDto editUserDto) {
            var user = await _db.AppUser.Where(a => a.AppUserId == editUserDto.AppUserId).FirstOrDefaultAsync() ?? throw new Exception();

            _mapper.Map(editUserDto, user);

            await _db.SaveChangesAsync();

        }

    }
}

