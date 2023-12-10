﻿using API.Logic.Services.AuthServiceLogic;
using Data.DTO;
using Microsoft.EntityFrameworkCore;
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
        var newUser = new RegisterDto() {
            Email = "test@test.com",
            UserName = "jimmybob",
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var token = await _authService.RegisterUserAsync(newUser);

        Assert.NotNull(token);
        Assert.Equal("testToken", token);
    }

    [Theory]
    [InlineData("test@test.com", "tim", "Password1@", "Password1@", "Invalid username formation")]
    [InlineData("testattest.com", "timjohnson", "Password1@", "Password1@", "Please provide valid email")]
    [InlineData("test1@test.com", "timjohnson", "password1@", "password1@", "Please provide valid password")]
    [InlineData("test1@test.com", "timjohnson", "Password1@", "Password!@", "Passwords do not match")]
    public async void Registration_Data_Checks(string email, string username, string password, string confirmPassword, string expectedError) {

        var newUser = new RegisterDto {
            Email = email,
            UserName = username,
            Password = password,
            ConfirmPassword = confirmPassword
        };

        try {

            var res = await _authService.RegisterUserAsync(newUser);
        }
        catch (Exception ex) {

            Assert.NotNull(ex);
            Assert.Equal(ex.Message, expectedError);
        }



    }

}