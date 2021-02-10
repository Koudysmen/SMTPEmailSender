
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
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
        private readonly string pathToFolderWhereTemplateAre;

        public EmailBodyBuilder(IMustacheReplacement mustacheReplacement, IConfiguration configuration)
        {
            this.mustacheReplacement = mustacheReplacement;
            this.pathToFolderWhereTemplateAre = configuration.GetValue<string>("EmailClient:PathToFolderWhereTemplateAre") 
                                                ?? throw new NullReferenceException("Missing PathToFolderWhereTemplateAre env variable in EmailClient section");
        }


        public MimeMessage CreateBodyAccordingEmailType(EmailSettings emailSettings, IDictionary<string, string> values)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings.MailBoxName, emailSettings.Sender));
            message.To.Add(new MailboxAddress(emailSettings.MailBoxName, emailSettings.Reciever));
            message.Subject = emailSettings.Subject;
            var builder = new BodyBuilder();

            var findEmailTemplate = this.ExistTemplate(pathToFolderWhereTemplateAre,
                emailSettings.HtmlTemplateNameWithoutExtension);

            if(!findEmailTemplate)
            {
                throw new KeyNotFoundException
                    ($"Template '{emailSettings.HtmlTemplateNameWithoutExtension}' not found in path {pathToFolderWhereTemplateAre}");
            }

            string path = this.CreateAbsoluteTemplatePath(pathToFolderWhereTemplateAre, emailSettings.HtmlTemplateNameWithoutExtension);

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

            return files.Any(_ => Path.GetFileNameWithoutExtension(_)
                .Equals(CheckHtmlTemplateName(templateName), StringComparison.OrdinalIgnoreCase));
        }

        
    }
}