using System;
using System.Linq;
using HonestyBar.Configuration.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HonestyBar.Configuration
{
    public static class CorsPolicies
    {
        public static IServiceCollection AddCorsFromConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var hosts = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            if (hosts.Any())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(CorsPolicyName.AllowConfiguredHosts, builder =>
                    {
                        builder.WithOrigins(hosts)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
                });
            }

            return services;
        }
    }
}