using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Core.Mapping
{
    public class HealthOrderMasterMap : EntityTypeConfiguration<HealthOrderMaster>
    {
        public HealthOrderMasterMap()
        {
            Property(h => h.Author).IsRequired();
            Property(h => h.DateTicks).IsRequired().HasMaxLength(30);
            Property(h => h.CheckBeginTime).IsOptional();
            Property(h => h.CheckEndTime).IsOptional();
            Property(h => h.BaokuConfirmDate).IsOptional();
            Property(h => h.CheckComConfirmDate).IsOptional();
            Property(h => h.FinanceConfirmDate).IsOptional();
            Property(h => h.FinancePayDate).IsOptional();
            HasOptional(h => h.Company).WithMany().HasForeignKey(h => h.CompanyId);
            HasRequired(h => h.HealthCheckProduct).WithMany().HasForeignKey(h => h.HealthCheckProductId);
        }
    }
}
