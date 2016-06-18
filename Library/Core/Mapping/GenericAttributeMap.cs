using Domain.Common;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
   public class GenericAttributeMap: EntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            ToTable("GenericAttribute");
            Property(c=>c.Description).IsOptional().HasMaxLength(50);
            Property(c => c.Key).IsRequired().HasMaxLength(20);
            Property(c => c.KeyGroup).IsRequired().HasMaxLength(20);
            Property(c => c.Value).IsRequired().HasMaxLength(50);
        }
    }
}
