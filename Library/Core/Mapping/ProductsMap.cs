using Domain.Products;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class ProductsMap : EntityTypeConfiguration<Product>
    {
        public ProductsMap()
        {
            ToTable("Products");
            Property(p => p.ItemNo).IsOptional().HasMaxLength(10);
            Property(p => p.ProdType).IsOptional().HasMaxLength(20);
            Property(p => p.SafeguardCode).IsOptional().HasMaxLength(20);
            Property(p => p.SafeguardName).IsOptional().HasMaxLength(50);
            Property(p => p.InsuredWho).IsOptional().HasMaxLength(10);
            Property(p => p.CoverageSum).IsOptional().HasMaxLength(10);
            Property(p => p.PayoutRatio).IsOptional().HasMaxLength(20);
            Property(p => p.HeadCount3).IsOptional().HasMaxLength(20);
            Property(p => p.HeadCount5).IsOptional().HasMaxLength(20);
            Property(p => p.HeadCount11).IsOptional().HasMaxLength(20);
            Property(p => p.HeadCount31).IsOptional().HasMaxLength(20);
            Property(p => p.HeadCount51).IsOptional().HasMaxLength(20);
            Property(p => p.HeadCount100).IsOptional().HasMaxLength(20);
            Property(p => p.InsuredCom).IsOptional().HasMaxLength(20);
            Property(p => p.ClaimCode).IsOptional().HasMaxLength(5);
        }
    }
}
