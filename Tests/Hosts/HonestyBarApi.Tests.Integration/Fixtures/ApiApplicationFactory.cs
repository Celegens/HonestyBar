using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace HonestyBar.Tests.Integration.Fixtures
{
    public abstract class ApiApplicationFactory<TEntryPoint>
        : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            return base.CreateServer(builder);
        }
    }
}