using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SMTPEmailSender.Model;
using SMTPEmailSender.Services;

namespace WebApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailBodyBuilder emailBodyBuilder;
        private readonly string smtpHost;
        private readonly string username;
        private readonly string password;
        private readonly int port;
        private readonly bool useSsl;

        public EmailService(IConfiguration configuration, IEmailBodyBuilder emailBodyBuilder)
        {
            this.smtpHost = configuration.GetValue<string>("Host") ?? throw new NullReferenceException("SMTP server cannot be null");
            this.username = configuration.GetValue<string>("Username") ?? throw new NullReferenceException("SMTP username cannot be null");
            this.password = configuration.GetValue<string>("Password") ?? throw new NullReferenceException("Password cannot be null");
            this.port = configuration.GetValue<int>("Port", 25);
            this.useSsl = configuration.GetValue<bool>("UseSsl", false);
            this.emailBodyBuilder = emailBodyBuilder;
        }

        public async Task SendMessage(EmailContent emailContent, IDictionary<string, string> values)
        {
            var message = this.emailBodyBuilder.CreateBodyAccordingEmailType(emailContent, values);

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(smtpHost, port, useSsl);
            await smtpClient.AuthenticateAsync(username, password);
            await smtpClient.SendAsync(message).ConfigureAwait(false);
            await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}