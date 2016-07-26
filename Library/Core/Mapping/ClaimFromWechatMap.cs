using Domain.Claim;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class ClaimFromWechatMap : EntityTypeConfiguration<ClaimFromWechatItem>
    {
        public ClaimFromWechatMap()
        {
            ToTable("ClaimFromWechat");
            HasMany(o => o.ImageList).WithRequired(pp => pp.claim).HasForeignKey(pp => pp.claim_Id);
            Property(o => o.Author).IsOptional().HasMaxLength(128);
            Property(o => o.CaseId).IsRequired().HasMaxLength(64);
            Property(o => o.CompanyName).IsOptional().HasMaxLength(64);
            Property(o => o.CreateTime).IsOptional().HasColumnType("datetime2");
            Property(o => o.Describe).IsOptional().HasMaxLength(512);
            Property(o => o.FullImage).IsOptional().HasMaxLength(512);
            Property(o => o.openid).IsOptional().HasMaxLength(256);
            Property(o => o.ProposerBirthday).IsOptional().HasColumnType("datetime2");
            Property(o => o.ProposerEmail).IsOptional().HasMaxLength(128);
            Property(o => o.ProposerIdNumber).IsOptional().HasMaxLength(32);
            Property(o => o.ProposerIdType).IsOptional().HasMaxLength(16);
            Property(o => o.ProposerName).IsOptional().HasMaxLength(32);
            Property(o => o.ProposerPhone).IsOptional().HasMaxLength(16);
            Property(o => o.ProposerSex).IsOptional().HasMaxLength(8);
            Property(o => o.RecipientBirthday).IsOptional().HasColumnType("datetime2");
            Property(o => o.RecipientEmail).IsOptional().HasMaxLength(16);
            Property(o => o.RecipientIdNumber).IsOptional().HasMaxLength(32);
            Property(o => o.RecipientIdType).IsOptional().HasMaxLength(16);
            Property(o => o.RecipientName).IsOptional().HasMaxLength(32);
            Property(o => o.RecipientPhone).IsOptional().HasMaxLength(16);
            Property(o => o.RecipientSex).IsOptional().HasMaxLength(8);
            Property(o => o.TransformToTPA).IsOptional();
        }
    }
}
