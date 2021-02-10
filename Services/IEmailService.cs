using SMTPEmailSender.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IEmailService
    {
        Task SendMessageAsync(EmailSettings emailSettings, IDictionary<string, string> customProperties);
    }
}
