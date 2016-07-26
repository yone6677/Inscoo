using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace FileSystem.Models
{
    public class ArchiveMap : EntityTypeConfiguration<ArchiveItem>
    {
        public ArchiveMap()
        {
            ToTable("BaseFile");
            Property(s => s.Type).IsRequired().HasMaxLength(64);
            Property(s => s.pId).IsOptional();
            Property(s => s.Name).IsRequired().HasMaxLength(64);
            Property(s => s.Path).IsRequired().HasMaxLength(256);
            Property(s => s.Memo).IsOptional().HasMaxLength(512);
            Property(s => s.Url).IsOptional().HasMaxLength(256);
            Property(s => s.Author).IsRequired().HasMaxLength(64);
            Property(s => s.Domain).IsOptional().HasMaxLength(256);
        }
    }
}