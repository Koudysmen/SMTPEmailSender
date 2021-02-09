
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MimeKit;
using SMTPEmailSender.Model;
using SMTPEmailSender.Services;
using WebApi.Services.EmailClient;

namespace WebApi.Services
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IMustacheReplacement mustacheReplacement;
        private static readonly string folderBreaker = "/";

        public EmailBodyBuilder(IMustacheReplacement mustacheReplacement)
        {
            this.mustacheReplacement = mustacheReplacement;
        }


        public MimeMessage CreateBodyAccordingEmailType(EmailContent emailContent, IDictionary<string, string> values)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailContent.MailBoxName, emailContent.Sender));
            message.To.Add(new MailboxAddress(emailContent.MailBoxName, emailContent.Reciever));
            message.Subject = emailContent.Subject;
            var builder = new BodyBuilder();

            var findEmailTemplate = this.ExistTemplate(emailContent.PathToFolderWhereTemplateAre,
                emailContent.HtmlTemplateNameWithoutExtension);

            if(!findEmailTemplate)
            {
                throw new KeyNotFoundException
                    ($"Template '{emailContent.HtmlTemplateNameWithoutExtension}' not found in path {emailContent.PathToFolderWhereTemplateAre}");
            }

            string path = this.CreateAbsoluteTemplatePath(emailContent.PathToFolderWhereTemplateAre, emailContent.HtmlTemplateNameWithoutExtension);

            builder.HtmlBody = this.mustacheReplacement.ReplaceVariablesInTemplate(values, path);

            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();
            return message;                 
        }

        private string CreateAbsoluteTemplatePath(string pathToFolderWhereTemplateAre, string templateName)
        {
            if (!pathToFolderWhereTemplateAre.EndsWith(folderBreaker))
            {
                pathToFolderWhereTemplateAre = string.Concat(pathToFolderWhereTemplateAre, folderBreaker);
            }

            templateName = this.CheckHtmlTemplateName(templateName);

            return string.Concat(pathToFolderWhereTemplateAre, templateName, ".html");
        }

        private string CheckHtmlTemplateName(string templateName)
        => templateName.StartsWith(folderBreaker) 
            ? templateName.Replace(folderBreaker, "")
            : templateName;



        private bool ExistTemplate(string templateName, string pathWhereTemplateAre)
        {
            if (!File.Exists(pathWhereTemplateAre))
            {
                throw new FileNotFoundException($"Path doesn't exist: {pathWhereTemplateAre}");
            }

            var files = Directory.GetFiles(pathWhereTemplateAre, "*.html").ToList();
            if(!files.Any())
            {
                throw new FileNotFoundException($"Not found any html templates in path: {pathWhereTemplateAre}");
            }

            return files.Any(_ => Path.GetFileNameWithoutExtension(_).Equals(templateName, StringComparison.OrdinalIgnoreCase));
        }

        
    }
}