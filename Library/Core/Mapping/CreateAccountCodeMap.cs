using Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class CreateAccountCodeMap : EntityTypeConfiguration<CreateAccountCode>
    {
        public CreateAccountCodeMap()
        {
            Property(p => p.AccountEncryCode).IsRequired().HasMaxLength(10);
            Property(p => p.EncryRoleName).HasMaxLength(50);
            Property(p => p.EncryCompanyName).HasMaxLength(150);
            Property(p => p.EncryCommissionMethod).HasMaxLength(100);
            Property(p => p.EncryInsurance).HasMaxLength(200);
            Property(p => p.EncrySeries).HasMaxLength(200);
            Property(p => p.EncryMemo).HasMaxLength(200);
            Property(p => p.EncryCreateID).HasMaxLength(50);
        }
    }
}
