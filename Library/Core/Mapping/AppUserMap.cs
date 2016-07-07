using Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class AppUserMap : EntityTypeConfiguration<AppUser>
    {
        public AppUserMap()
        {
            Property(p => p.TiYong).IsRequired();
            Property(p => p.FanBao).IsRequired();
            Property(p => p.Rebate).IsOptional();
            Property(p => p.CreaterId).IsOptional().HasMaxLength(128);
            Property(p => p.Changer).IsOptional().HasMaxLength(128);
            Property(p => p.Email).IsRequired().HasMaxLength(32);
            Property(p => p.CompanyName).HasMaxLength(128);
            Property(p => p.LinkMan).IsOptional().HasMaxLength(32);
            Property(p => p.Ident).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Rebate).IsRequired();
            Property(p => p.BankName).IsOptional().HasMaxLength(50);
            Property(p => p.BankNumber).IsOptional().HasMaxLength(50);

            Property(p => p.UserName).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true }));
        }
    }
}
