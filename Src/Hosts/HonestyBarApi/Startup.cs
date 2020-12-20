using HonestyBar.Configuration;
using HonestyBar.Configuration.Constants;
using HonestyBar.Domain;
using HonestyBar.Filters.Actions;
using HonestyBar.Infrastructure.EntityFrameworkCore;
using HonestyBar.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace HonestyBar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApplicationInsightsTelemetry()
                .AddCorsFromConfiguration(Configuration)
                .AddCorrelationIdConfiguration()
                .AddHttpClientConfiguration()
                .AddApiDocumentation()
                .AddApiVersioningConfiguration()
                .AddVersionedApiExplorer(x =>
                {
                    x.GroupNameFormat = "'v'VVV"; // Version format: 'v'major[.minor][-status]
                    x.SubstituteApiVersionInUrl = true;
                })
                .AddControllers(options =>
                {
                    options.Filters.Add<SerilogControllerLoggingFilter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddTransient(_ =>
            {
                return new DbContextOptionsBuilder<HonestyBarUnitOfwork>()
                .UseInMemoryDatabase("HonestyBarDb")
                .Options;
            });
            services.AddScoped<HonestyBarUnitOfwork>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();


            SeedDatabase(services.BuildServiceProvider().GetService<HonestyBarUnitOfwork>());
        }

        private static void SeedDatabase(HonestyBarUnitOfwork database)
        {
            database.AddRange(
                new Employee("Geert", "Schreyers", "geert.schreyers@kenze.be"),
                new Employee("Kenny", "Cornelissen", "kenny.cornelissen@kenze.be")
                );

            database.AddRange(
                new Product("Coca cola",1.60d),
                new Product("Mineral water",1.20d)
                ); ;

            database.SaveChanges();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorrelationIdConfiguration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            // UseCors needs to be called after UseRouting but before other middleware that depends on it (e.g. UseAuthentication)
            app.UseCors(CorsPolicyName.AllowConfiguredHosts);

            // Uncomment the following line(s) if you require authentication or authorization.
            // Keep in mind that this call needs to be done AFTER UseRouting
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseApiDocumentation();
        }
    }
}
