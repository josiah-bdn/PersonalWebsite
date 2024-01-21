using API.Logic.Services.AuthServiceLogic;
using Data.DTO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var inMemorySettings = new Dictionary<string, string>
             {
                 {"JWT_SECRET_KEY", "zivmxwYN+TzH+oOl7pqB1jvRjnwjAJG5NKMJXp8Of8WU4SrxX+o9ykMWWI7cyuHG\ngPOvvHT0gVvajO7+4OX+Jg=="},

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

        [Fact]
        public async Task Successful_ValidateResetCodeAsync_Attempt() {
            var user = await CreateUser();
            var passwordReset = await CreatePasswordResetEntity(user.AppUserId);

            var validateRequest = new ResetCodeConfirmation {
                Email = user.Email,
                Code = passwordReset.Code
            };

            await _authService.ValidateResetCodeAsync(validateRequest);

            var res = await _db.PasswordResetRequests.FirstOrDefaultAsync();

            Assert.NotNull(res);
            Assert.True(res.UserHasValidated);

        }

        [Fact]
        public async Task Successful_ResetPasswordAysnc_Attempt() {
            var user = await CreateUser();
            var passwordReset = await CreatePasswordResetEntity(user.AppUserId);

            var validateRequest = new ResetCodeConfirmation {
                Email = user.Email,
                Code = passwordReset.Code
            };

            await _authService.ValidateResetCodeAsync(validateRequest);

            var reset = new ResetPasswordDto {
                Email = user.Email,
                Password = "StrongPassword1@",
                ConfirmPassword = "StrongPassword1@"
            };

            var resetRes = await _authService.ResetPasswordAysnc(reset);
            var authRes = await _db.Authentication.FirstOrDefaultAsync();

            Assert.NotNull(authRes);
            Assert.NotNull(resetRes);
            Assert.NotEmpty(resetRes);
            Assert.Equal(2, authRes.LoginCount);

        }

        [Fact]
        public async Task ValidDateResetCode_Exceeds_Attempts() {
            var user = await CreateUser();
            await CreatePasswordResetEntity(user.AppUserId);

            var validateRequest = new ResetCodeConfirmation {
                Email = user.Email,
                Code = 9999
            };

            int validationAttempts = 4;

            for (int i = 0; i < validationAttempts; i++) {

                try {
                    await _authService.ValidateResetCodeAsync(validateRequest);
                }
                catch (Exception ex) {
                    Assert.NotEmpty(ex.Message);
                }
            }

            var passwordRes = _db.PasswordResetRequests.FirstOrDefault();

            Assert.NotNull(passwordRes);
            Assert.Equal(3, passwordRes.CodeEntryCount);
            Assert.True(passwordRes.IsExpiredOrFailed);
            Assert.False(passwordRes.UserHasValidated);

        }

        [Fact]
        public async Task ValidateResetCode_Exceeds_Time_Limit() {
            var user = await CreateUser();
            var resetRequest = await CreatePasswordResetEntity(user.AppUserId);

            var backDatedTime = DateTime.UtcNow.AddMinutes(-11);

            resetRequest.SendDate = backDatedTime;
            _db.Update(resetRequest);
            await _db.SaveChangesAsync();

            var validateRequest = new ResetCodeConfirmation {
                Email = user.Email,
                Code = 1111
            };
            try {
                await _authService.ValidateResetCodeAsync(validateRequest);
            }
            catch (Exception ex) {
                Assert.NotEmpty(ex.Message);
                Assert.Equal("Reset code expired, please request new code", ex.Message);
            }

        }

        [Fact]
        public async Task Unsuccessful_RequestPassword_Reset() {

            try {

                await _authService.SendResetEmailAsync("someemail@test.com");

            }
            catch (Exception ex) {

                Assert.NotNull(ex);
                Assert.Equal("Email does not exits", ex.Message);

            }

        }

        private async Task<PasswordResetRequest> CreatePasswordResetEntity(Guid userId) {
            var passwordReset = new PasswordResetRequest {
                PasswordRequestId = Guid.NewGuid(),
                AppUserId = userId,
                Code = 1111,
                SendDate = DateTime.UtcNow,
                UserHasValidated = false,
                CodeEntryCount = 0,
                IsExpiredOrFailed = false

            };

            _db.Add(passwordReset);
            await _db.SaveChangesAsync();
            return passwordReset;

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

