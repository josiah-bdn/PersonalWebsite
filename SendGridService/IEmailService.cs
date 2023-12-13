using System;
namespace SendGridService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}

