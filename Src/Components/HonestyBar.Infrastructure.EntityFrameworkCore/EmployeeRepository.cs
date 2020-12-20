using HonestyBar.Domain;
using HonestyBar.Infrastructure.EntityFrameworkCore;
using HonestyBar.Infrastructure.Interfaces;
using HonestyBar.Infrastructure.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _unitOfWork.Set<Employee>().ToListAsync();
        }

        public Employee Add(Employee employee)
        {
            _unitOfWork.Set<Employee>().Add(employee);

            return employee;
        }

        public async Task<Employee> FindAsync(Guid id)
        {
            return await _unitOfWork.Set<Employee>().FindAsync(id);
        }

    }
}
