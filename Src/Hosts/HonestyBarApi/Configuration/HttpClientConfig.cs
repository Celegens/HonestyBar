using System;
using CorrelationId.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace HonestyBar.Configuration
{
    public static class HttpClientConfig
    {
        public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient("DefaultHttpClient")
                .AddCorrelationIdForwarding()
                .AddTransientHttpErrorPolicy(policy =>
                {
                    return policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600));
                });

            return services;
        }
    }
}
