using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HonestyBar.Domain
{
    public class ConsumptionConfiguration : IEntityTypeConfiguration<Consumption>
    {
        public void Configure(EntityTypeBuilder<Consumption> builder)
        {
            builder.HasKey(e => e.Id); 
            builder.Property(e => e.DateTime).IsRequired();
  
        }
    }
}
