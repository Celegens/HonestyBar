using HonestyBar.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HonestyBar.Infrastructure.Interfaces.Repositories
{
    public interface IProductRepository: IRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> FindAsync(Guid id);

        Product Add(Product product);
    }
}
