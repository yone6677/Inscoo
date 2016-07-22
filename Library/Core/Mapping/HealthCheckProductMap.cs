using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Core.Mapping
{
    public class HealthCheckProductMap : EntityTypeConfiguration<HealthCheckProduct>
    {
        public HealthCheckProductMap()
        {
            Property(p => p.Author).IsRequired();
        }
    }
}
