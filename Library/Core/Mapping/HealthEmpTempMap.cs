using Domain.Health;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
   public class HealthEmpTempMap :EntityTypeConfiguration<HealthEmpTemp>
    {
        public HealthEmpTempMap()
        {
            Property(h => h.Author).IsRequired();
            Property(h => h.Birthday).IsOptional().HasColumnType("datetime2");
            Property(h => h.Address).IsRequired().HasMaxLength(128);
            Property(h => h.Name).IsRequired().HasMaxLength(32);
            Property(h => h.IdNumber).IsRequired().HasMaxLength(20);
            Property(h => h.Phone).IsRequired().HasMaxLength(16);
        }
    }
}
