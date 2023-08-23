using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.File;
using Domain.Entities.Invoices;
using Domain.Entities.Users;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services.Email;

public sealed class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;

    public EmailService(EmailConfiguration emailConfiguration, IConfiguration configuration, IFileService fileService)
    {
        _emailConfiguration = emailConfiguration;
        _configuration = configuration;
        _fileService = fileService;
    }

    private async Task SendEmailAsync(MailboxAddress from, MailboxAddress to, string subject, string content, CancellationToken cancellationToken, MimePart? attachment = null)
    {
       MimeMessage message = ConstructEmail(from, to, subject, content, attachment);

        using SmtpClient client = new();

        await client.ConnectAsync(_emailConfiguration.SMTP, _emailConfiguration.Port, _emailConfiguration.UseTLS, cancellationToken);
        await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private static MimeMessage ConstructEmail(MailboxAddress from, MailboxAddress to, string subject, string content, MimePart? attachment)
    {
        MimeMessage message = new();
        TextPart body = new("html")
        {
            Text = content
        };

        message.From.Add(from);
        message.To.Add(to);

        message.Subject = subject;

        if (attachment is not null)
        {
            Multipart multipart = new("mixed")
            {
                body,
                attachment
            };

            message.Body = multipart;
            return message;
        }
        
        message.Body = body;

        return message;
    }

    public async Task SendNoReplyMailAsync(User user, string subject, string content, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);

        await SendEmailAsync(from, to, subject, content, cancellationToken);
    }

    public async Task SendRegistrationMailAsync(User user, string token, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        string spaUrl = _configuration["SPA:Url"];

        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);
        string content = $"<a href='{spaUrl}/activate/{token}'>Activate account</a>";
        string subject = "Regstration confirmation";

        await SendEmailAsync(from, to, subject, content, cancellationToken);
    }

    public async Task SendLoginVerificationMailAsync(User user, int verificationCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);
        string content = $"Vaš verifikacijski kod je {verificationCode}";
        string subject = "Verifikacijski kod";

        await SendEmailAsync(from, to, subject, content, cancellationToken);
    }

    public Task SendIssuedTicketsAsync(User user, Invoice invoice, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);

        byte[] pdf = _fileService.GenerateIssuedTicketPDF(invoice);
        MemoryStream memoryStream = new(pdf);

        MimePart mimePart = new("application", "pdf")
        {
            Content = new MimeContent(memoryStream, ContentEncoding.Default),
            ContentDisposition = new(MimeKit.ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = "Tickets.pdf"
        };

        string content = "Your tickets.";
        string subject = "Tickets";

        return SendEmailAsync(from, to, subject, content, cancellationToken, mimePart);
    }

    public Task SendInvoiceAsync(User user, Invoice invoice, CancellationToken cancellationToken)
    {
        MailboxAddress from = new("No-reply", _emailConfiguration.From);
        MailboxAddress to = new($"{user.FirstName} {user.LastName}", user.Email);

        byte[] pdf = _fileService.GenerateInvoicePDF(invoice);
        MemoryStream memoryStream = new(pdf);

        MimePart mimePart = new("application", "pdf")
        {
            Content = new MimeContent(memoryStream, ContentEncoding.Default),
            ContentDisposition = new(MimeKit.ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = "Invoice.pdf"
        };

        string content = "Your invoice.";
        string subject = "Invoice";

        return SendEmailAsync(from, to, subject, content, cancellationToken, mimePart);
    }
}
