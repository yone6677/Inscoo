using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class BaseFileMap : EntityTypeConfiguration<BaseFile>
    {
        public BaseFileMap()
        {
            Property(s => s.Name).IsRequired().HasMaxLength(64);
            Property(s => s.Path).IsRequired().HasMaxLength(256);
            Property(s => s.Memo).IsOptional().HasMaxLength(512);
            Property(s => s.Url).IsOptional().HasMaxLength(256);
            Property(s => s.Author).IsRequired().HasMaxLength(64);
        }
    }
    public class ArchiveMap : EntityTypeConfiguration<Archive>
    {
        public ArchiveMap()
        {
            Property(s => s.Type).IsRequired().HasMaxLength(64);
            Property(s => s.pId).IsOptional();
        }
    }
    public class BusinessLicenseMap : EntityTypeConfiguration<BusinessLicense>
    {
        public BusinessLicenseMap()
        {
            //HasRequired(b => b.Company).WithMany().HasForeignKey(b => b.CompanyId);
        }
    }
}
