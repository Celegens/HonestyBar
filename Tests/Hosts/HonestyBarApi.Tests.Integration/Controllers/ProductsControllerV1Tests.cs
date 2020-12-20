using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HonestyBar.Domain;
using HonestyBar.Tests.Integration.Fixtures;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace HonestyBar.Tests.Integration
{
    public class ProductsControllerV1Tests
        : ApiApplicationFactory<Startup>
    {
        private readonly HttpClient _client;


        public ProductsControllerV1Tests()
        {
            _client = CreateClient();
        }

        [Fact]
        public async Task Options_ProductsRoot_Returns200Ok()
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Options, "api/v1/products");
            var response = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                new string[] { HttpMethods.Get, HttpMethods.Head, HttpMethods.Options, HttpMethods.Post },
                response.Content.Headers.Allow);
        }


     

    }
}
