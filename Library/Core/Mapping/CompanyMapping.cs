using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain;

namespace Core.Mapping
{
    public class CompanyMapping : EntityTypeConfiguration<Company>
    {
        public CompanyMapping()
        {
            Property(c => c.Name).HasMaxLength(100).IsRequired();
            Property(c => c.Address).HasMaxLength(100).IsRequired();
            Property(c => c.Code).HasMaxLength(10).IsUnicode(false).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true }));
            Property(c => c.Email).HasMaxLength(100).IsOptional();
            Property(c => c.Phone).HasMaxLength(30).IsRequired();
            Property(c => c.LinkMan).HasMaxLength(100).IsRequired();

            HasRequired(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            HasOptional(c => c.BusinessLicenseFile).WithMany().HasForeignKey(c => c.BusinessLicenseFileId);
        }
    }
}
