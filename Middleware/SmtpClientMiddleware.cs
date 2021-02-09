using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMTPEmailSender.Model;
using SMTPEmailSender.Services;
using WebApi.Services;
using WebApi.Services.EmailClient;

namespace SMTPEmailSender.Middleware
{
    internal static class SmtpClientMiddleware
    {
        public static IServiceCollection AddEmailClient(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSection = configuration.GetSection("EmailClient");
            if (!emailSection.Exists())
            {
                throw new MissingFieldException("Missing 'EmailClient' section in your appsettings.json file");
            }

            
            // configure DI for application services
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IEmailBodyBuilder, EmailBodyBuilder>();
            services.AddSingleton<IMustacheReplacement, MustacheReplacement>();
            return services;
        }
    }
}
