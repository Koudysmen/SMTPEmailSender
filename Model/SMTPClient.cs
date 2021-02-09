using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMTPEmailSender.Model
{
    public class SMTPClient
    {
        [Required(ErrorMessage = "Host cannot be null")]
        public string Host { get; set; }

        [Required(ErrorMessage = "Port cannot be 0")]
        public int Port { get; set; } = 465;

        [Required(ErrorMessage = "Username cannot be null")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Password cannot be null")]
        public string Password { get; set; }

        public bool UseSsl { get; set; } = true;
    }
}
