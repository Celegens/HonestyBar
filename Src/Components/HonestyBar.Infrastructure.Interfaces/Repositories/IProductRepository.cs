using HonestyBar.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HonestyBar.Infrastructure.Interfaces.Repositories
{
    public interface IProductRepository: IRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

        Task<Product> FindAsync(Guid id,CancellationToken cancellationToken);

        Product Add(Product product);
    }
}
