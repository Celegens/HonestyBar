using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProductRepository _productRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IProductRepository productRepository)
        {
            _employeeRepository = employeeRepository;
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
        /// <param name="employeeId">The value's identifier.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
#pragma warning disable IDE0060 // Remove unused parameter
        public IActionResult Options(Guid employeeId)
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
            var employees = await _employeeRepository.GetAllAsync(); 
            return new OkObjectResult(employees);
        }

        [HttpGet("{employeeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.FindAsync(employeeId, cancellationToken);

            return new OkObjectResult(new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Email));
        }
        [HttpPost("{employeeId}/addconsumption/{productId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> AddConsumptionAsync(Guid employeeId, Guid productId, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.FindAsync(employeeId, cancellationToken).ConfigureAwait(false);
            var product = await _productRepository.FindAsync(productId, cancellationToken).ConfigureAwait(false);

            employee.Consumptions.Add(new Consumption(product));
            employee.Saldo -= product.UnitPrice;
             await _employeeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

              return new OkObjectResult(employee);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> PostAsync([FromBody] CreateEmployeeDto createEmployeeDto, CancellationToken cancellationToken)
        {
            var employee = new Employee(createEmployeeDto.FirstName, createEmployeeDto.LastName, createEmployeeDto.Email);

            employee = _employeeRepository.Add(employee);

            await _employeeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = employee.Id }, new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Email));
        }
    }
}

