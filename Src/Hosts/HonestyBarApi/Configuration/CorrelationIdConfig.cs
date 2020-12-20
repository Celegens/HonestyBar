using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HonestyBar.Configuration
{
    /// <summary>
    /// Add CorrelationId middleware to the ASPNET pipeline.
    /// See https://github.com/stevejgordon/CorrelationId for more information.
    /// </summary>
    public static class CorrelationIdConfig
    {
        public static IServiceCollection AddCorrelationIdConfiguration(this IServiceCollection services)
        {
            services.AddDefaultCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
            });

            return services;
        }

        public static IApplicationBuilder UseCorrelationIdConfiguration(this IApplicationBuilder app)
        {
            app.UseCorrelationId();
            return app;
        }
    }
}