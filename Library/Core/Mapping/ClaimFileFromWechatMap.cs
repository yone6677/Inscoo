using Domain.Claim;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class ClaimFileFromWechatMap : EntityTypeConfiguration<ClaimFileFromWechatItem>
    {
        public ClaimFileFromWechatMap()
        {
            ToTable("claimFileFromWechat");
            Property(o => o.Author).IsOptional().HasMaxLength(128);
        }
    }
}
