using Application.Abstractions.Email;
using Domain.Entities.Korisnici;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services.Email;

public sealed class EmailHandler : IEmailHandler
{
    private readonly EmailConfiguration _configuration;

    public EmailHandler(EmailConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendNoReplyMail(Korisnik user, string subject, string content)
    {
        //MimeMessage message = new();
        //message.From.Add(new MailboxAddress("No-reply", _configuration.From));
        //message.To.Add(new MailboxAddress($"{user.Ime} {user.Prezime}", user.Email));

        //message.Subject = subject;
        //message.Body = new TextPart("html")
        //{
        //    Text = content
        //};

        //using (SmtpClient client = new())
        //{
        //    await client.ConnectAsync(_configuration.SMTP, _configuration.Port, _configuration.UseTLS);
        //    await client.AuthenticateAsync(_configuration.Username, _configuration.Password);
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}

        await File.WriteAllTextAsync("dump.txt", content);
    }

    public Task SendRegistrationMail(Korisnik user)
    {
        throw new NotImplementedException();
    }
}
