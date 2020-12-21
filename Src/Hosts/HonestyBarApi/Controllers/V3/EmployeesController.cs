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

namespace HonestyBar.Controllers.V3
{
    [Route("v{version:apiVersion}/[controller]")]
     [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion(ApiVersionNames.V3)]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProductRepository _productRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IProductRepository productRepository)
        {
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
        }

        [HttpPost("PayOfficeManager/{employeeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> PayOfficeManager(Guid employeeId,double saldo, CancellationToken cancellationToken)
        {
            if(saldo > 0) { 
            var employee = await _employeeRepository.FindAsync(employeeId, cancellationToken);
                employee.Saldo += saldo; 
                await _employeeRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return new OkObjectResult(employee.Saldo);
            }
            else
            {
                return new BadRequestResult();
            }
        }


        [HttpGet("GetSaldo/{employeeId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetSaldo(Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.FindAsync(employeeId, cancellationToken);

            return new OkObjectResult(employee.Saldo);
        }
    }
}

