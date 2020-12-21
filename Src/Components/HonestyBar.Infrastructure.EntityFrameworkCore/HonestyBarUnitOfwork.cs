using HonestyBar.Domain;
using HonestyBar.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading; 
using System.Threading.Tasks;

namespace HonestyBar.Infrastructure.EntityFrameworkCore
{
    public class HonestyBarUnitOfwork: DbContext, IUnitOfWork
    {
        public HonestyBarUnitOfwork(DbContextOptions<HonestyBarUnitOfwork> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ConsumptionConfiguration());
            modelBuilder.Entity<Consumption>().Property(e => e.Id).ValueGeneratedNever();

        }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken);
        }
    }
}
