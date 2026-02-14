using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityApp.Services;

/// <summary>
/// Email sender service for Identity email confirmations and password resets
/// In development, emails are logged to console
/// </summary>
public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sends an email message (logs to console in development)
    /// </summary>
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // In development, log the email to console
        _logger.LogInformation($"""
            
            ========== EMAIL SENT ==========
            To: {email}
            Subject: {subject}
            
            {htmlMessage}
            
            ================================
            
            """);

        // In production, you would implement actual email sending here
        // Example using SendGrid, SMTP, etc.
        return Task.CompletedTask;
    }
}
