using HonestyBar.Domain;
using HonestyBar.Infrastructure.EntityFrameworkCore;
using HonestyBar.Infrastructure.Interfaces;
using HonestyBar.Infrastructure.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HonestyBar
{
    public class ProductRepository : IProductRepository
    {
        private readonly HonestyBarUnitOfwork _unitOfWork;

        public ProductRepository(HonestyBarUnitOfwork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.Set<Product>().ToListAsync(cancellationToken);
        }

        public async Task<Product> FindAsync(Guid id,CancellationToken cancellationToken)
        {
            return await _unitOfWork.Set<Product>().FindAsync(new object[] { id }, cancellationToken);
        }

        public Product Add(Product product)
        {
            _unitOfWork.Set<Product>().Add(product);

            return product;
        }
    }
}
