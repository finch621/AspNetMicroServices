using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastracture;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastracture.Mail;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _client;
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSettings _settings;

    public EmailService(ISendGridClient client, ILogger<EmailService> logger, IOptions<EmailSettings> settings)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings.Value;
    }

    public async Task<bool> SendEmail(Email email)
    {
        var msg = new SendGridMessage
        {
            From = new EmailAddress(_settings.FromAddress, _settings.FromName),
            Subject = email.Subject,
        };

        msg.AddTo(email.To);
        msg.AddContent(MimeType.Text, email.Body);

        var response = await _client.SendEmailAsync(msg).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
            return true;

        _logger.LogError("Something went wrong while sending mail");

        return false;
    }
}
