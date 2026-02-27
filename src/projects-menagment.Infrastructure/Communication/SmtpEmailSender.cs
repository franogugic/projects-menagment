using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using projects_menagment.Application.Interfaces.Communication;

namespace projects_menagment.Infrastructure.Communication;

public sealed class SmtpEmailSender(
    IOptions<SmtpOptions> smtpOptions,
    ILogger<SmtpEmailSender> logger) : IEmailSender
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

    public async Task SendOrganizationInviteAsync(
        string toEmail,
        string organizationName,
        string inviteLink,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_smtpOptions.Host) ||
            string.IsNullOrWhiteSpace(_smtpOptions.FromEmail) ||
            IsPlaceholderValue(_smtpOptions.Host) ||
            IsPlaceholderValue(_smtpOptions.FromEmail) ||
            IsPlaceholderValue(_smtpOptions.Username) ||
            IsPlaceholderValue(_smtpOptions.Password))
        {
            logger.LogWarning(
                "SMTP is not configured. Invite mail was not sent to {Email}. Link: {InviteLink}",
                toEmail,
                inviteLink);
            return;
        }

        try
        {
            using var message = new MailMessage(_smtpOptions.FromEmail, toEmail)
            {
                Subject = $"Invitation to join {organizationName}",
                Body = $"You have been invited to join {organizationName}. Click the link to accept: {inviteLink}",
                IsBodyHtml = false
            };

            using var smtpClient = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
            {
                EnableSsl = _smtpOptions.EnableSsl,
                Timeout = 10_000
            };

            if (!string.IsNullOrWhiteSpace(_smtpOptions.Username))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password);
            }

            using var ctr = cancellationToken.Register(() => smtpClient.SendAsyncCancel());
            await smtpClient.SendMailAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "Failed to send invite email to {Email}. Returning invite link in API response instead. Link: {InviteLink}",
                toEmail,
                inviteLink);
        }
    }

    private static bool IsPlaceholderValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        var normalized = value.Trim();
        return normalized.Contains("YOUR_", StringComparison.OrdinalIgnoreCase) ||
               normalized.Contains("your-provider", StringComparison.OrdinalIgnoreCase);
    }
}
