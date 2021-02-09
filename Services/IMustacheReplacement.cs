using System;
using System.Collections.Generic;
using System.Text;

namespace SMTPEmailSender.Services
{
    public interface IMustacheReplacement
    {
        string ReplaceVariablesInTemplate(IDictionary<string, string> values, string templatePath);
    }
}
