using Domain.Finance;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class CashFlowDetailsMap : EntityTypeConfiguration<CashFlowDetails>
    {
        public CashFlowDetailsMap()
        {
            ToTable("CashFlowDetails");
            HasKey(d => d.Id);
            Property(d => d.Receivable).IsRequired();
            Property(d => d.Payable).IsRequired();
            Property(d => d.ActualCollected).IsRequired();
            Property(d => d.RealPayment).IsRequired();
            Property(d => d.Author).IsRequired().HasMaxLength(256);
            Property(d => d.TransferVoucher).IsOptional().HasMaxLength(256);
            Property(d => d.Memo).IsOptional().HasMaxLength(512);
        }
    }
}
