using Domain.Finance;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class CashFlowMap : EntityTypeConfiguration<CashFlow>
    {
        public CashFlowMap()
        {
            ToTable("CashFlow");
            HasMany(c => c.cashFlowDetails).WithRequired(pp => pp.cashFlow).HasForeignKey(pp => pp.cId);
            Property(c => c.Amount).IsRequired();
            Property(c => c.Difference).IsRequired();
            Property(d => d.Author).IsRequired().HasMaxLength(256);
        }
    }
}
