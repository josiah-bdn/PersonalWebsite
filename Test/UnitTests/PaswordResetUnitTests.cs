using API.Logic.Services.AuthServiceLogic;
using Castle.Core.Configuration;
using Data.DTO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Moq;
using Persistence;
using SendGridService;

namespace Test.UnitTests {
    public class PaswordResetUnitTests {
        private readonly DataContext _db;
        private readonly AuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly Mock<IEmailService> _mockEmailService;

        public PaswordResetUnitTests() {
            var options = new DbContextOptionsBuilder<DataContext>()
              .UseInMemoryDatabase(databaseName: "TestAuthDb")
              .EnableSensitiveDataLogging()
               .Options;


            var inMemorySettings = new Dictionary<string, string>
             {
                 {"JWT_SECRET", "zivmxwYN+TzH+oOl7pqB1jvRjnwjAJG5NKMJXp8Of8WU4SrxX+o9ykMWWI7cyuHG\ngPOvvHT0gVvajO7+4OX+Jg=="},

            };

            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();


            _tokenService = new TokenService(_configuration);

            _db = new DataContext(options);


            _mockEmailService = new Mock<IEmailService>();


            _authService = new AuthService(_db, _tokenService, _mockEmailService.Object);
        }

        [Fact]
        public async Task Successful_RequestPassword_Reset() {
            var user = await CreateUser();

            await _authService.SendResetEmailAsync(user.Email);

            _mockEmailService.Verify(
             service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
             Times.Once
            );

            var resetRequest = await _db.PasswordResetRequests.FirstOrDefaultAsync();
            Assert.NotNull(resetRequest);
            Assert.Equal(user.AppUserId, resetRequest.AppUserId);
            Assert.False(resetRequest.IsExpiredOrFailed);
            Assert.False(resetRequest.UserHasValidated);
        }

        private async Task<AppUser> CreateUser() {
            var user = new AppUser {
                AppUserId = Guid.NewGuid(),
                Email = "resettest@dispostable.com",
                UserName = "resettester12",
                FirstName = "Jimbob",
                LastName = "Jones",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };

            var (hashPassword, passwordSalt) = _tokenService.HashPassword("Password1@");

            var auth = new Authentication {
                AppUserId = user.AppUserId,
                HashPassword = hashPassword,
                PasswordSalt = passwordSalt,
                LoginCount = 1,
                LastLogin = DateTime.UtcNow,
            };

            _db.Add(user);
            _db.Add(auth);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}

