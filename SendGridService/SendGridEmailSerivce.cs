using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendGridService;

public class SendGridEmailService : IEmailService {
    private readonly SendGridClient _client;

    public SendGridEmailService(string apiKey) {
        _client = new SendGridClient(apiKey);
    }

    public async Task SendEmailAsync(string to, string subject, string body) {

        var message = new SendGridMessage() {
            From = new EmailAddress("josiahunderwood88@gmail.com", "Josiah"),
            Subject = subject,
            PlainTextContent = body,
            HtmlContent = body
        };

        message.AddTo(new EmailAddress(to));
        await _client.SendEmailAsync(message);
    }
}

