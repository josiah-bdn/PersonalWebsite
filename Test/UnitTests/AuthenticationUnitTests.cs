using API.Logic.Services.AuthServiceLogic;
using Data.DTO;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using SendGridService;

namespace Test.UnitTests;

public class AuthenticationUntitTests {
    private readonly DataContext _db;
    private readonly AuthService _authService;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IEmailService> _mockEmailService;

    public AuthenticationUntitTests() {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestAuthDb")
            .EnableSensitiveDataLogging()
            .Options;


        _mockTokenService = new Mock<ITokenService>();
        _db = new DataContext(options);

        _mockTokenService.Setup(service => service.GenerateJwtToken(It.IsAny<Guid>()))
                         .Returns("testToken");

        _mockTokenService.Setup(service => service.HashPassword(It.IsAny<string>()))
                         .Returns(() => ("mockedHash", "mockedSalt"));

        _mockEmailService = new Mock<IEmailService>();


        _authService = new AuthService(_db, _mockTokenService.Object, _mockEmailService.Object);
    }

    [Fact]
    public async Task Successful_Registration_Token() {
        var newUser = new RegisterDto() {
            Email = "test@test.com",
            UserName = "jimmybob",
            FirstName = "John",
            LastName = "Approve",
            Password = "Password1@",
            ConfirmPassword = "Password1@"
        };

        var token = await _authService.RegisterUserAsync(newUser);

        Assert.NotNull(token);
        Assert.Equal("testToken", token);
    }

    [Theory]
    [InlineData("test@test.com", "tim", "John", "Approve", "Password1@", "Password1@", "Invalid username formation")]
    [InlineData("testattest.com", "timjohnson", "John", "Approve", "Password1@", "Password1@", "Please provide valid email")]
    [InlineData("test1@test.com", "timjohnson", "John", "Approve", "password1@", "password1@", "Please provide valid password")]
    [InlineData("test1@test.com", "timjohnson", "John", "Approve", "Password1@", "Password!@", "Passwords do not match")]
    [InlineData("test1@test.com", "timjohnson", "T", "Approve", "Password1@", "Password1@", "Provide valid first or last name")]
    public async void Registration_Data_Checks(string email, string username, string firstName, string lastName, string password, string confirmPassword, string expectedError) {

        var newUser = new RegisterDto {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
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