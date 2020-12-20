using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HonestyBar.Tests.Integration.Fixtures;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HonestyBar.Tests.Integration
{
    public class CorsPolicyTests
        : ApiApplicationFactory<Startup>
    {
        private readonly HttpClient _client;

        public CorsPolicyTests()
        {
            _client = CreateClient();
        }

        [Fact]
        public async Task Allows_cross_origin_from_configured_hosts()
        {
            var config = Server.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            var hosts = config?.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

            foreach (var host in hosts)
            {
                using var message = new HttpRequestMessage(HttpMethod.Get, "");
                message.Headers.Add("Origin", host);
                var response = await _client.SendAsync(message).ConfigureAwait(false);

                Assert.Equal(host, response.Headers.GetValues("Access-Control-Allow-Origin").FirstOrDefault());
            }
        }

        [Fact]
        public async Task Denies_cross_origin_from_unknown_hosts()
        {
            const string host = "http://example.com";
            using var message = new HttpRequestMessage(HttpMethod.Get, "");
            message.Headers.Add("Origin", host);
            var response = await _client.SendAsync(message).ConfigureAwait(false);

            Assert.False(response.Headers.TryGetValues("Access-Control-Allow-Origin", out _));
        }
    }
}