using Data.DTO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Text.RegularExpressions;
using API.ExceptionHandlers;
using Data.Enum;
using SendGridService;
using System.Text;

namespace API.Logic.Services.AuthServiceLogic {
    public class AuthService : IAuthService {
        private readonly DataContext _db;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private Random _random = new Random();

        public AuthService(DataContext db, ITokenService tokenService, IEmailService emailService) {
            _db = db;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<string> RegisterUserAsync(RegisterDto register) {

            ValidateRegisterData(register);

            if (await _db.AppUser.AnyAsync(u => u.Email == register.Email || u.UserName == register.UserName))
                throw new AppException(ErrorCode.AuthenticationError, "Username or password already in use.");

            var user = new AppUser {
                AppUserId = Guid.NewGuid(),
                Email = register.Email,
                UserName = register.UserName,
                CreatedDate = DateTime.UtcNow
            };

            var (hashPassword, passwordSalt) = _tokenService.HashPassword(register.Password);

            var authentication = new Authentication {
                AppUserId = user.AppUserId,
                HashPassword = hashPassword,
                PasswordSalt = passwordSalt,
                LastLogin = DateTime.UtcNow,
                LoginCount = 0
            };

            _db.AppUser.Add(user);
            _db.Authentication.Add(authentication);
            await _db.SaveChangesAsync();

            return _tokenService.GenerateJwtToken(user.AppUserId);

        }

        public async Task<string> LoginAsync(LoginDto login) {
            var user = await GetAppUserAsync(login.Email);

            var auth = await _db.Authentication.FirstOrDefaultAsync(a => a.AppUserId == user.AppUserId);

            if (auth == null) {
                throw new AppException(ErrorCode.AuthenticationError, "Authentication invalid.");
            }

            if (!_tokenService.ValidatePassword(login.Password, auth.HashPassword, auth.PasswordSalt)) {
                throw new AppException(ErrorCode.AuthenticationError, "Password does not meet stated requirements");
            }

            auth.LoginCount += 1;
            _db.Authentication.Update(auth);
            await _db.SaveChangesAsync();

            return _tokenService.GenerateJwtToken(user.AppUserId);
        }


        public async Task SendResetEmailAsync(string email) {
            var user = await GetAppUserAsync(email);

            var code = GeneratePasswordResetCode();
            var subject = "Reset Password";
            var body = CreateResetPasswordBody(user.FirstName, code);

            var resetRequest = new PasswordResetRequest {
                PasswordRequestId = Guid.NewGuid(),
                AppUserId = user.AppUserId,
                Code = code,
                SendDate = DateTime.UtcNow,
                UserHasValidated = false,
            };

            _db.Add(resetRequest);
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(email, subject, body);
        }

        public async Task ValidateResetCodeAsync(ResetCodeConfirmation reset) {
            var userId = await _db.AppUser.Where(u => u.Email == reset.Email).Select(u => u.AppUserId).FirstOrDefaultAsync();

            var resetInfo = await _db.PasswordResetRequests
                .Where(p => p.AppUserId == userId)
                .OrderByDescending(p => p.SendDate)
                .FirstOrDefaultAsync();

            if (resetInfo == null || resetInfo.IsExpiredOrFailed == true)
                throw new AppException(ErrorCode.AuthenticationError, "No reset request found, please reattempt");

            if (reset.Code != resetInfo.Code) {
                if (resetInfo.CodeEntryCount < 3) {
                    resetInfo.CodeEntryCount++;
                    await UpdatePasswordResetEntity(resetInfo);
                    throw new AppException(ErrorCode.AuthenticationError, "Codes do not match");
                }

                resetInfo.IsExpiredOrFailed = true;
                await UpdatePasswordResetEntity(resetInfo);

                throw new AppException(ErrorCode.AuthenticationError, "Reset attempts exceeds maximum amount, please request new code");
            }

            if ((DateTime.UtcNow - resetInfo.SendDate).TotalMinutes > 10) {
                resetInfo.IsExpiredOrFailed = true;
                await UpdatePasswordResetEntity(resetInfo);

                throw new AppException(ErrorCode.AuthenticationError, "Reset code expired, please request new code");
            }

            resetInfo.UserHasValidated = true;
            _db.Update(resetInfo);
            await _db.SaveChangesAsync();
        }

        public async Task<string> ResetPasswordAysnc(ResetPasswordDto reset) {

            if (reset.Password != reset.ConfirmPassword)
                throw new AppException(ErrorCode.AuthenticationError, "Password entries do not match");

            if (!IsValidPassword(reset.Password)) throw new AppException(ErrorCode.AuthenticationError, "Please provide valid password");

            var user = await GetAppUserAsync(reset.Email);

            bool hasValidated = await _db.PasswordResetRequests
             .Where(p => p.AppUserId == user.AppUserId)
             .OrderByDescending(p => p.SendDate)
             .Select(p => p.UserHasValidated).FirstOrDefaultAsync();

            if (!hasValidated) throw new AppException(ErrorCode.AuthenticationError, "User has not validated reset");

            var (hashPassword, passwordSalt) = _tokenService.HashPassword(reset.Password);

            var authentication = await _db.Authentication.Where(a => a.AppUserId == user.AppUserId).FirstOrDefaultAsync();

            if (authentication is null) throw new AppException(ErrorCode.AuthenticationError, "Authentication record could not be found");

            authentication.HashPassword = hashPassword;
            authentication.PasswordSalt = passwordSalt;
            authentication.LastLogin = DateTime.UtcNow;
            authentication.LoginCount++;

            _db.Authentication.Update(authentication);
            await _db.SaveChangesAsync();

            return _tokenService.GenerateJwtToken(user.AppUserId);

        }

        private async Task<AppUser> GetAppUserAsync(string email) {
            var user = await _db.AppUser.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user is null)
                throw new AppException(ErrorCode.AuthenticationError, "Email does not exits");

            return user;
        }

        private async Task UpdatePasswordResetEntity(PasswordResetRequest req) {
            _db.Update(req);
            await _db.SaveChangesAsync();
        }

        private string CreateResetPasswordBody(string? firstName, int code) {

            var body = new StringBuilder();

            body.AppendLine($"Dear {firstName},");
            body.AppendLine("<br>");
            body.AppendLine("You have requested to reset your password.");
            body.AppendLine("<br><br>");

            body.AppendLine("Please click on the link below to reset your password, you will need the below 4 digit code");
            body.AppendLine("<br>");

            body.AppendLine(code.ToString());
            body.AppendLine("<br>");

            body.AppendFormat("<a href=\"{0}\">Reset Password</a>", "http://localhost:3000");
            body.AppendLine("<br><br>");

            body.AppendLine("If you did not request a password reset, please ignore this email or contact support if you have concerns.");
            body.AppendLine("<br>");
            body.AppendLine("Best regards,");
            body.AppendLine("<br>");
            body.AppendLine("Josiah");

            return body.ToString();

        }

        private int GeneratePasswordResetCode() {

            int min = 1000;
            int max = 9999;

            return _random.Next(min, max);
        }

        private void ValidateRegisterData(RegisterDto register) {
            if (register.Password != register.ConfirmPassword) throw new AppException(ErrorCode.AuthenticationError, "Passwords do not match");

            if (!IsValidEmail(register.Email)) throw new AppException(ErrorCode.AuthenticationError, "Please provide valid email");

            if (!IsValidUsername(register.UserName)) throw new AppException(ErrorCode.AuthenticationError, "Invalid username formation");

            if (!IsValidPassword(register.Password)) throw new AppException(ErrorCode.AuthenticationError, "Please provide valid password");

        }

        private bool IsValidEmail(string email) {
            if (string.IsNullOrEmpty(email)) {
                return false;
            }

            var emailPattern = @"^\S+@\S+\.\S+$";

            var multipleDotsPaterrn = @"(\.\.)|(@\.)|(\.@)";

            return Regex.IsMatch(email, emailPattern) && !Regex.IsMatch(email, multipleDotsPaterrn);
        }

        private bool IsValidUsername(string username) {
            if (string.IsNullOrWhiteSpace(username)) {
                return false;
            }

            int minLength = 5;
            int maxLength = 20;

            var allowedUsernamePattern = "^[a-zA-Z0-9$!@]{5,20}$";
            ;

            if (username.Length < minLength || username.Length > maxLength) {
                return false;
            }

            return Regex.IsMatch(username, allowedUsernamePattern);
        }

        private bool IsValidPassword(string password) {
            if (string.IsNullOrWhiteSpace(password)) {
                return false;
            }

            int minLength = 8;
            int maxLength = 15;

            var hasNumberPattern = @"[0-9]+";
            var hasSpecialCharPattern = @"[$@!#%^&*]+";
            var hasUpperCasePattern = @"[A-Z]+";


            if (password.Length < minLength || password.Length > maxLength) {
                return false;
            }

            bool hasNumber = Regex.IsMatch(password, hasNumberPattern);
            bool hasSpecialChar = Regex.IsMatch(password, hasSpecialCharPattern);
            bool hasUpperCaseLetter = Regex.IsMatch(password, hasUpperCasePattern);

            return hasNumber && hasSpecialChar && hasUpperCaseLetter;
        }
    }

}
