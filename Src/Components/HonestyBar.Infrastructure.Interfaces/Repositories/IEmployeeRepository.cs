using HonestyBar.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HonestyBar.Infrastructure.Interfaces.Repositories
{
    public interface IEmployeeRepository: IRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();

        Task<Employee> FindAsync(Guid id);

        Employee Add(Employee employee);
    }
}
