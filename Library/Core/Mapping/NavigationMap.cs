using Domain.Navigation;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class NavigationMap : EntityTypeConfiguration<Navigation>
    {
        public NavigationMap()
        {
            ToTable("Navigation");
            Property(c => c.action).IsOptional().HasMaxLength(64);
            Property(c => c.controller).IsOptional().HasMaxLength(64);
            Property(c => c.isShow).IsRequired();
            Property(c => c.level).IsRequired();
            Property(c => c.memo).IsOptional().HasMaxLength(256);
            Property(c => c.name).IsRequired().HasMaxLength(16);
            Property(c => c.pId).IsOptional();
            Property(c => c.url).IsOptional().HasMaxLength(128);
            Property(c => c.htmlAtt).IsOptional().HasMaxLength(256);
            Property(c => c.sequence).IsOptional();
            Ignore(c => c.SonMenu);
        }
    }
}
