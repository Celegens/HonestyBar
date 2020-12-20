using System;
using System.IO;
using System.Linq;
using System.Reflection;
using HonestyBar.Filters.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HonestyBar.Configuration
{
    public static class ApiDocumentation
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var assembly = typeof(Startup).Assembly;
                var assemblyProduct = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
                var assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

                c.DescribeAllParametersInCamelCase();

                c.OperationFilter<CorrelationIdOperationFilter>();

                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                {
                    var info = new OpenApiInfo()
                    {
                        Title = assemblyProduct,
                        Description = apiVersionDescription.IsDeprecated ?
                            $"{assemblyDescription} This API version has been deprecated." :
                            assemblyDescription,
                        Version = apiVersionDescription.ApiVersion.ToString(),
                        Contact = new OpenApiContact()
                        {
                            Name = "Kenze CVBA",
                            Email = "info@kenze.be",
                            Url = new Uri("https://kenze.be")
                        }
                    };

                    c.SwaggerDoc(apiVersionDescription.GroupName, info);
                }


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Set the Swagger UI browser document title.
                options.DocumentTitle = typeof(Startup)
                    .Assembly
                    .GetCustomAttribute<AssemblyProductAttribute>()
                    .Product;
                // Set the Swagger UI to render at '/'.
                options.RoutePrefix = string.Empty;
                // Show the request duration in Swagger UI.
                options.DisplayRequestDuration();

                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                foreach (var apiVersionDescription in provider
                    .ApiVersionDescriptions
                    .OrderByDescending(x => x.ApiVersion))
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        $"Version {apiVersionDescription.ApiVersion}");
                }

            });

            return app;
        }
    }
}