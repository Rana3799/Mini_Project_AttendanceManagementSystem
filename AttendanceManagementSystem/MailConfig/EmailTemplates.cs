namespace AttendanceManagementSystem.MailConfig
{    public static class EmailTemplates
    {
        public static string GetResetPasswordHtml(string displayName, string resetUrl)
        {
            return $@"
            <html>
            <body>
                <p>Hi {System.Net.WebUtility.HtmlEncode(displayName)},</p>
                <p>We received a request to reset your password. Click the link below to reset it:</p>
                <p><a href=""{resetUrl}"" target=""_blank"">Reset your password</a></p>
                <p>If you didn't request this, you can ignore this email.</p>
                <hr/>
                <p>Regards,<br/>Your Team</p>
            </body>
            </html>";
        }
    }

}
