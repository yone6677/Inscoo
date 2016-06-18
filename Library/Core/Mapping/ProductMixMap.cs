using Domain.Products;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class ProductMixMap : EntityTypeConfiguration<MixProduct>
    {
        public ProductMixMap()
        {
            ToTable("ProductMix");
            HasMany(p => p.ProductMixItem).WithRequired(pp => pp.prodMix).HasForeignKey(pp => pp.mid);
            Property(i => i.Name).IsRequired().HasMaxLength(20);
            Property(i => i.Code).IsRequired().IsUnicode(false).HasMaxLength(10).IsFixedLength();
            Property(i => i.Description).IsOptional().HasMaxLength(256);
            Property(i => i.Price).IsRequired();
            Property(p => p.Address).IsRequired().HasMaxLength(50);
            Property(p => p.AgeRange).IsRequired().HasMaxLength(50);
            Property(p => p.StaffRange).IsRequired().HasMaxLength(50);
        }
    }
}
