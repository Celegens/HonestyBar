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

namespace HonestyBar.Controllers.V2
{
    [Route("v{version:apiVersion}/[controller]")]
     [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion(ApiVersionNames.V2)]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProductRepository _productRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IProductRepository productRepository)
        {
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
        }

        [HttpPost("deactivate/{employeeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> DeActivateAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.FindAsync(employeeId, cancellationToken);
            employee.Active = false; 
            await _employeeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return new OkObjectResult(new EmployeeDto(employee.Id, employee.FirstName, employee.LastName, employee.Email));
        }


        [HttpGet("ListOfEmployees/{status}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetListOfEmployees(bool status, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync(status, cancellationToken);
           
            return new OkObjectResult(employees.Select(e => new EmployeeDto(e.Id, e.FirstName, e.LastName, e.Email)));
        }
    }
}

