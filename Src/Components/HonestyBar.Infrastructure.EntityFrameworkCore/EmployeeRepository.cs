using HonestyBar.Domain;
using HonestyBar.Infrastructure.EntityFrameworkCore;
using HonestyBar.Infrastructure.Interfaces;
using HonestyBar.Infrastructure.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HonestyBar
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HonestyBarUnitOfwork _unitOfWork;

        public EmployeeRepository(HonestyBarUnitOfwork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public async Task<IEnumerable<Employee>> GetAllAsync(bool active = true, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Set<Employee>().Include(s => s.Consumptions).ThenInclude(c=>c.Product).Where(e => e.Active == active).ToListAsync();
        }

        public Employee Add(Employee employee)
        {
            _unitOfWork.Set<Employee>().Add(employee);

            return employee;
        }

        public async Task<Employee> FindAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Set<Employee>().FindAsync(new object[] { id }, cancellationToken);
        }

      
    }
}
