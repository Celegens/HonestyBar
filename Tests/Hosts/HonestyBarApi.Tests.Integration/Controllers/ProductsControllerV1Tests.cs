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


        [Fact]
        public async Task AddConsumption_Returns200Ok()
        { 
            var responseProduct = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/v1/products")).ConfigureAwait(false);
            var responseContentProduct = await responseProduct.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<Product[]>(responseContentProduct);
             
            var responseEmployee = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/v1/employee")).ConfigureAwait(false);
            var responseContentEmployee = await responseEmployee.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<Employee[]>(responseContentEmployee);

            var employeeId = employees[0].Id;
            var productId = products[0].Id;

            var employeeSaldo = employees[0].Saldo;
            var productPrice = products[0].UnitPrice;

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Options, $"api/v1/{employeeId}/addconsumption/{productId}");
            var response = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(responseContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(employee.Saldo, employeeSaldo - productPrice);
        }

    }
}
