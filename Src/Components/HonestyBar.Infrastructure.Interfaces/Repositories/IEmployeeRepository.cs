using HonestyBar.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HonestyBar.Infrastructure.Interfaces.Repositories
{
    public interface IEmployeeRepository: IRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync(bool active = true, CancellationToken cancellationToken = default);

        Task<Employee> FindAsync(Guid id,CancellationToken cancellationToken = default);

        Employee Add(Employee employee );
    }
}
