using System;
using System.Collections.Generic;
using System.Text;
using MimeKit;
using SMTPEmailSender.Model;

namespace SMTPEmailSender.Services
{
    public interface IEmailBodyBuilder
    {
        MimeMessage CreateBodyAccordingEmailType(EmailContent emailContent, IDictionary<string, string> values);
    }
}
