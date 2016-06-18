using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class AppRoleMap : EntityTypeConfiguration<AppRole>
    {
        public AppRoleMap()
        {
            Property(c => c.Description).IsOptional().HasMaxLength(128);
        }
    }
}
