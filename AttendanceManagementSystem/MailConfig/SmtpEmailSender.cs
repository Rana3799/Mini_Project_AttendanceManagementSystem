using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace AttendanceManagementSystem.MailConfig
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IOptions<SmtpSettings> options, ILogger<SmtpEmailSender> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;  // For development, avoid SSL issues

                await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl);
                if (!string.IsNullOrEmpty(_settings.User))
                    await client.AuthenticateAsync(_settings.User, _settings.Password);

                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {To}", to);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
