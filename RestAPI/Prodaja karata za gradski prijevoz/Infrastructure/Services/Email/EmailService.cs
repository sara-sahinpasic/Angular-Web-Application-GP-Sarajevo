using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Email;
using Domain.Entities.Korisnici;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

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

    private async Task SendEmail(MailboxAddress from, MailboxAddress to, string subject, string content, CancellationToken cancellationToken)
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

    public async Task SendNoReplyMail(Korisnik to, string subject, string content, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SendRegistrationMail(Korisnik user, string token, CancellationToken cancellationToken)
    {
        string spaUrl = _configuration["SPA:Url"];

        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.Ime} {user.Prezime}", user.Email);
        string content = $"<a href='{spaUrl}/activate/{token}'>Activate account</a>";
        string subject = "Regstration confirmation";

        await SendEmail(from, to, subject, content, cancellationToken);
    }

    public async Task SendLoginVerificationMail(Korisnik user, int verificationCode, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.Ime} {user.Prezime}", user.Email);
        string content = $"Vaš verifikacijski kod je {verificationCode}";
        string subject = "Verifikacijski kod";

        await SendEmail(from, to , subject, content, cancellationToken);
    }
}
