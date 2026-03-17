using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MMN.Util.Util
{
    public class EmailExceptionHandler
    {
        private readonly string _sendGridApiKey;
        private readonly string _fromEmail;
        private readonly string _toEmail;

        public EmailExceptionHandler()
        {
            _sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? string.Empty;
            _fromEmail = "contato@quantashop.com.br";
            _toEmail = "ericmb10@live.com";
        }

        public async Task SendExceptionEmailAsync(Exception ex, string message, string email)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress(_fromEmail, "Quanta Shop");
            var subject = "An exception occurred";
            var to = new EmailAddress(_toEmail);
            var plainTextContent = GenerateEmailBody(ex, message, email);
            var htmlContent = $"<pre>{plainTextContent}</pre>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            Console.WriteLine(response.StatusCode);
        }

        private string GenerateEmailBody(Exception ex, string message, string email)
        {
            return $@"
            An exception has occurred:
            From: {email}
            Message: {ex.Message}
            Message: {message}
            Stack Trace: {ex.StackTrace}
            {(ex.InnerException != null ? "Inner Exception: " + ex.InnerException.Message : string.Empty)}";
        }
    }
}