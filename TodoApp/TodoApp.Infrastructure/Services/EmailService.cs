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
    private readonly CancellationTokenSource _appShutdownTokenSource;
    public EmailService(IOptions<SmtpSettings> smtpSettings, CancellationTokenSource appShutdownTokenSource)
    {
        _smtpSettings = smtpSettings.Value;
        _appShutdownTokenSource = appShutdownTokenSource;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        if (string.IsNullOrEmpty(toEmail)) return;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls, _appShutdownTokenSource.Token);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password, _appShutdownTokenSource.Token);
            await client.SendAsync(message, _appShutdownTokenSource.Token);
            await client.DisconnectAsync(true, _appShutdownTokenSource.Token);
        }
    }
}