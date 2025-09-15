namespace AttendanceManagementSystem.MailConfig
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlBody);
    }
}
