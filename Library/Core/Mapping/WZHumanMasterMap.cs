using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class WZHumanMasterMap : EntityTypeConfiguration<WZHumanMaster>
    {
        public WZHumanMasterMap()
        {
            Property(p => p.Account).IsRequired().HasMaxLength(100);
            Property(p => p.CompanyName).IsRequired().HasMaxLength(100);
            Property(p => p.InsuranceBeginTime).IsOptional();
            Property(p => p.InsuranceEndTime).IsOptional();
        }
    }
}
