using System;
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
    [Route(ApiRoutes.DefaultApiControllerRoute)]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion(ApiVersionNames.V1)]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
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
        public async Task<IActionResult> GetAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            return new OkObjectResult(employees.Select(e => new EmployeeDto(e.Id, e.FirstName, e.LastName, e.Email)));
        }

        [HttpGet("{employeeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByIdAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.FindAsync(employeeId);

            return new OkObjectResult(new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Email));
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

