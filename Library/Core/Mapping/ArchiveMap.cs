using Domain.Archives;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class ArchiveMap : EntityTypeConfiguration<Archive>
    {
        public ArchiveMap()
        {
            ToTable("Archives");
            Property(s => s.Name).IsRequired().HasMaxLength(64);
            Property(s => s.Type).IsRequired().HasMaxLength(64);
            Property(s => s.Path).IsRequired().HasMaxLength(256);
            Property(s => s.Memo).IsOptional().HasMaxLength(512);
            Property(s => s.Url).IsOptional().HasMaxLength(256);
            Property(s => s.pId).IsOptional();
            Property(s => s.Author).IsRequired().HasMaxLength(64);
        }
    }
}
