using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using SMTPEmailSender.Model;

namespace WebApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailBodyBuilder emailBodyBuilder;
        private readonly string smtpHost;
        private readonly string username;
        private readonly string password;
        private readonly int port;
        private readonly bool useSSL;

        public EmailService(SMTPClient emailClient)
        {
            this.smtpHost = emailClient.Host;
            this.username = emailClient.Username;
            this.password = emailClient.Password;
            this.port = emailClient.Port;
            this.useSSL = emailClient.UseSSL;
            this.emailBodyBuilder = new EmailBodyBuilder();
        }

        public async Task SendMessage(EmailContent emailContent, IDictionary<string, string> values)
        {
            var message = this.emailBodyBuilder.CreateBodyAcordingEmailType(emailContent, values);

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(smtpHost, port, useSSL);
            await smtpClient.AuthenticateAsync(username, password);
            await smtpClient.SendAsync(message).ConfigureAwait(false);
            await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}