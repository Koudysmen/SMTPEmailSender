using Stubble.Core.Builders;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebApi.Services.EmailClient
{
    internal class MustacheReplacement
    {
        
        public string ReplaceVariablesInTemplate(IDictionary<string, string> values, string templatePath)
        {
            var stubble = new StubbleBuilder().Build();
            using (StreamReader streamReader = new StreamReader(templatePath, Encoding.UTF8))
            {
                var output = stubble.Render(streamReader.ReadToEnd(), values);
                return output;
            }
        }
    }
}
