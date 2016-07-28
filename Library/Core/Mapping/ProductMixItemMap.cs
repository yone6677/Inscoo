using Domain.Products;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class ProductMixItemMap : EntityTypeConfiguration<MixProductItem>
    {
        public ProductMixItemMap()
        {
            ToTable("ProductMixItem");
            Property(a => a.CoverageSum).IsRequired().HasMaxLength(50);
            Property(a => a.PayoutRatio).IsOptional().HasMaxLength(50);
            Property(a => a.OriginalPrice).IsRequired();
            Property(a => a.SafefuardName).IsRequired().HasMaxLength(50);
        }
    }
}
