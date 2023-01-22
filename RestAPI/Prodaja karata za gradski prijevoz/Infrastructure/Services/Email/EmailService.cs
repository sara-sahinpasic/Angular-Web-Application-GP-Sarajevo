using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Email;
using Domain.Entities.Korisnici;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services.Email;

public sealed class EmailService : IEmailService
{
    private readonly EmailConfiguration _configuration;

    public EmailService(EmailConfiguration configuration)
    {
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
            await client.ConnectAsync(_configuration.SMTP, _configuration.Port, _configuration.UseTLS, cancellationToken);
            await client.AuthenticateAsync(_configuration.Username, _configuration.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }

    public async Task SendNoReplyMail(string to, string subject, string content, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public async Task SendNoReplyMail(Korisnik to, string subject, string content, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SendRegistrationMail(Korisnik user, string token, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _configuration.From);
        MailboxAddress to = new($"{user.Ime} {user.Prezime}", user.Email);
        string content = $"<a href='http://localhost:5192/account/activate/{token}'>Activate account</a>";
        string subject = "Regstration confirmation";

        await SendEmail(from, to, subject, content, cancellationToken);
    }
}
