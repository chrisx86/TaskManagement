#nullable enable
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        if (string.IsNullOrEmpty(toEmail)) return;

        //var message = new MimeMessage();
        //message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        //message.To.Add(new MailboxAddress("", toEmail));
        //message.Subject = subject;

        //var bodyBuilder = new BodyBuilder { HtmlBody = body };
        //message.Body = bodyBuilder.ToMessageBody();

        //using (var client = new SmtpClient())
        //{
        //    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        //    await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        //    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}
    }
}