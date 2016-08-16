using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class ClaimFilesListMap : EntityTypeConfiguration<ClaimFilesList>
    {
        public ClaimFilesListMap()
        {
            Ignore(i => i.Id);
            HasKey(i => i.ClaimFilesListID);
            Property(o => o.ClaimFilesBatchCode).IsRequired().HasMaxLength(50);
            Property(o => o.ClaimFilesName).IsRequired().HasMaxLength(50);
            Property(o => o.ClaimFilesStatus).IsRequired().HasMaxLength(1);
            Property(o => o.Author).IsRequired().HasColumnName("ClaimFilesCreateID");
            Property(o => o.CreateTime).IsRequired().HasColumnName("ClaimFilesCreateTime");
        }
    }
}
