using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Email;
using Domain.Entities.Users;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services.Email;

public sealed class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly IConfiguration _configuration;

    public EmailService(EmailConfiguration emailConfiguration, IConfiguration configuration)
    {
        _emailConfiguration = emailConfiguration;
        _configuration = configuration;
    }

    private async Task SendEmailAsync(MailboxAddress from, MailboxAddress to, string subject, string content, CancellationToken cancellationToken)
    {
        MimeMessage message = new();
        message.From.Add(from);
        message.To.Add(to);

        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = content
        };

        using (SmtpClient client = new())
        {
            await client.ConnectAsync(_emailConfiguration.SMTP, _emailConfiguration.Port, _emailConfiguration.UseTLS, cancellationToken);
            await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }

    public async Task SendNoReplyMailAsync(User user, string subject, string content, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);

        await SendEmailAsync(from, to, subject, content, cancellationToken);
    }

    public async Task SendRegistrationMailAsync(User user, string token, CancellationToken cancellationToken)
    {
        string spaUrl = _configuration["SPA:Url"];

        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);
        string content = $"<a href='{spaUrl}/activate/{user.Id}/{token}'>Activate account</a>";
        string subject = "Regstration confirmation";

        await SendEmailAsync(from, to, subject, content, cancellationToken);
    }

    public async Task SendLoginVerificationMailAsync(User user, int verificationCode, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);
        string content = $"Vaš verifikacijski kod je {verificationCode}";
        string subject = "Verifikacijski kod";

        await SendEmailAsync(from, to , subject, content, cancellationToken);
    }
}
