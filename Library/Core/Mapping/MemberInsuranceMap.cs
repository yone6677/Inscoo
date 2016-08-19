using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class MemberInsuranceMap : EntityTypeConfiguration<MemberInsurance>
    {
        public MemberInsuranceMap()
        {
            Property(p => p.EOrderCode).HasMaxLength(50);
            Property(p => p.UniqueKey).HasMaxLength(50);
            Property(p => p.Status).HasMaxLength(5);
            Property(p => p.PdfFileName).HasMaxLength(100);
            Property(p => p.Author).IsRequired().HasMaxLength(100);
            Property(p => p.FileTypeName).IsRequired().HasMaxLength(50);


            HasRequired(c => c.Excel).WithMany().HasForeignKey(c => c.ExcelId);
            HasOptional(c => c.Einsurance).WithMany().HasForeignKey(c => c.EinsuranceId);
        }
    }
}
