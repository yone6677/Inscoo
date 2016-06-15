using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
