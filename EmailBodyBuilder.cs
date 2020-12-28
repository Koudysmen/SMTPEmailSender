
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MimeKit;
using SMTPEmailSender.Model;
using WebApi.Services.EmailClient;

namespace WebApi.Services
{
    public class EmailBodyBuilder
    {
        private readonly MustacheReplacement mustacheReplacement;

        public EmailBodyBuilder()
        {
            this.mustacheReplacement = new MustacheReplacement();
        }


        public MimeMessage CreateBodyAcordingEmailType(EmailContent emailContent, IDictionary<string, string> values)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Poziciam Si", emailContent.Sender));
            message.To.Add(new MailboxAddress("Poziciam Si", emailContent.Reciever));
            message.Subject = emailContent.Subject;
            var builder = new BodyBuilder();

            var findEmailTemplate = this.ExistTemplate(emailContent.PathToFolderWhereTemplateAre,
                emailContent.HtmlTemplateNameWithoutExtension);

            if(!findEmailTemplate)
            {
                throw new KeyNotFoundException
                    ($"Template '{emailContent.HtmlTemplateNameWithoutExtension}' not found in path {emailContent.PathToFolderWhereTemplateAre}");
            }

            string path = string.Concat(emailContent.PathToFolderWhereTemplateAre,
                emailContent.HtmlTemplateNameWithoutExtension, ".html");

            builder.HtmlBody = this.mustacheReplacement.ReplaceVariablesInTemplate(values, path);

            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();
            return message;                 
        }

        private bool ExistTemplate(string templateName, string pathWhereTemplateAre)
        {
            if (!File.Exists(pathWhereTemplateAre))
            {
                return false;
            }

            var files = Directory.GetFiles(pathWhereTemplateAre).ToList();
            if(files.Any())
            {
                return false;
            }

            return files.Any(_ => 
                Path.GetFileNameWithoutExtension(_).Equals(templateName, StringComparison.OrdinalIgnoreCase));
        }

        
    }
}