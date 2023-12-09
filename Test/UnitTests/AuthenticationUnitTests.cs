using API.Logic.Services.AppUserLogic;
using API.Logic.Services.AuthServiceLogic;
using AutoMapper;
using Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Persistence;

namespace Test;

public class AuthenticationUntiTests {
    private readonly DataContext _db;
    private readonly AuthService _authService;
    private readonly Mock<ITokenService> _mockTokenService;

    public AuthenticationUntiTests() {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestAuthDb")
            .EnableSensitiveDataLogging() // Add this line
            .Options;


        _mockTokenService = new Mock<ITokenService>();
        _db = new DataContext(options);

        _mockTokenService.Setup(service => service.GenerateJwtToken(It.IsAny<Guid>()))
                         .Returns("testToken");

        _mockTokenService.Setup(service => service.HashPassword(It.IsAny<string>()))
                         .Returns(() => ("mockedHash", "mockedSalt"));


        _authService = new AuthService(_db, _mockTokenService.Object);
    }

    [Fact]
    public async void Successful_Registration_Token() {
        var newUser = GenerateRegisterDto();

        var token = await _authService.RegisterUserAsync(newUser);

        Assert.NotNull(token);
        Assert.Equal("testToken", token);

    }

    private RegisterDto GenerateRegisterDto() {
        return new RegisterDto {
            Email = "test@test.com",
            UserName = "jimmybob",
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };
    }

}