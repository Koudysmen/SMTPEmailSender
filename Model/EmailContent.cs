using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMTPEmailSender.Model
{
    public class EmailSettings
    {
        [Required(ErrorMessage = "{0} cannot be null")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0} cannot be null")]
        [EmailAddress]
        public string Reciever { get; set; }

        [Required(ErrorMessage = "{0} cannot be null")]
        [EmailAddress]
        public string Sender { get; set; }

        [Required(ErrorMessage = "{0} cannot be null")]
        public string MailBoxName { get; set; }

        [Required(ErrorMessage = "{0} cannot be null")]
        public string HtmlTemplateNameWithoutExtension { get; set; }

    }
}
