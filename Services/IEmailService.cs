using SMTPEmailSender.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IEmailService
    {
        Task SendMessage(EmailContent emailContent, IDictionary<string, string> customProperties);
    }
}
