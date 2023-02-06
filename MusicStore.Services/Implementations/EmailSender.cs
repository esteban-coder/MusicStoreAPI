using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MusicStore.Services.Implementations;

public class EmailSender : IEmailSender
{
    private readonly IOptions<AppSettings> _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<AppSettings> options, ILogger<EmailSender> logger)
    {
        _options = options;
        _logger = logger;
    }
    
    public async Task SendEmailAsync(EmailMessageInfo message)
    {
        var client = new SendGridClient(_options.Value.SmtpConfiguration.ApiKey);

        try
        {
            var from = new EmailAddress(_options.Value.SmtpConfiguration.From, _options.Value.SmtpConfiguration.FromName);

            var to = new EmailAddress(message.To, message.ToName);
            var plainTextContent = message.Body;

            var htmlContent = $"<p><strong>{plainTextContent}</strong></p>";

            var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);

            _logger.LogInformation("El email se envio correctamente a {To}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando el correo electrónico a {email}", message.To);
        }
    }
}