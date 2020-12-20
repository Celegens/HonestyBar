using System.Threading;
using System.Threading.Tasks;

namespace HonestyBar.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
