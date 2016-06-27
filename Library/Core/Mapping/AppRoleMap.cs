using System.Data.Entity.ModelConfiguration;
using Domain;
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
