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
    public class ProductRepository : IProductRepository
    {
        private readonly HonestyBarUnitOfwork _unitOfWork;

        public ProductRepository(HonestyBarUnitOfwork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _unitOfWork.Set<Product>().ToListAsync();
        }

        public async Task<Product> FindAsync(Guid id)
        {
            return await _unitOfWork.Set<Product>().FindAsync(id);
        }

        public Product Add(Product product)
        {
            _unitOfWork.Set<Product>().Add(product);

            return product;
        }
    }
}
