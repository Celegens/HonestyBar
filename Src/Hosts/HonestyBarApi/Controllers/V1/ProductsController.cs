using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using HonestyBar.Configuration.Constants;
using HonestyBar.Contracts.Requests;
using HonestyBar.Contracts.Responses;
using HonestyBar.Domain;
using HonestyBar.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace HonestyBar.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion(ApiVersionNames.V1)]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Options()
        {
            HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);

            return Ok();
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods for a value with the specified identifier.
        /// </summary>
        /// <param name="productId">The value's identifier.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
#pragma warning disable IDE0060 // Remove unused parameter
        public IActionResult Options(Guid productId)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                //HttpMethods.Delete,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options
                //HttpMethods.Patch,
                //HttpMethods.Post,
                //HttpMethods.Put
                );

            return Ok();
        }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return new OkObjectResult(products.Select(p => new ProductDto(p.Id, p.Name)));
        }

        [HttpGet("{productId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindAsync(productId, cancellationToken);

            return new OkObjectResult(new ProductDto(product.Id, product.Name));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> PostAsync([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            var product = new Product(createProductDto.Name); 
            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = product.Id } , new ProductDto(product.Id, product.Name));
        }
    }
}

