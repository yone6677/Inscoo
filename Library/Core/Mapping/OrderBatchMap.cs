using Domain.Orders;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
   public class OrderBatchMap: EntityTypeConfiguration<OrderBatch>
    {
        public OrderBatchMap()
        {
            ToTable("OrderBatch");
            Property(b => b.order_Id).IsRequired();
            Property(b => b.AmountCollected).IsOptional();
            Property(b => b.Author).IsOptional().HasMaxLength(64);
            Property(b => b.BNum).IsOptional().HasMaxLength(32);
            Property(b => b.BStatus).IsOptional().HasMaxLength(16);
            Property(b => b.CollectionDate).IsOptional().HasColumnType("datetime2");
            Property(b => b.CourierNumber).IsOptional().HasMaxLength(32);
            Property(b => b.EmpInfoFile).IsOptional();
            Property(b => b.EmpInfoFilePDF).IsOptional();
            Property(b => b.EmpInfoFileSeal).IsOptional();
            Property(b => b.Finance).IsOptional().HasMaxLength(64);
            Property(b => b.FinanceDate).IsOptional().HasColumnType("datetime2");
            Property(b => b.FinanceMemo).IsOptional().HasMaxLength(512);
            Property(b => b.InscooConfirm).IsOptional().HasMaxLength(64);
            Property(b => b.InscooConfirmDate).IsOptional().HasColumnType("datetime2");
            Property(b => b.Insurer).IsOptional().HasMaxLength(64);
            Property(b => b.InsurerConfirmDate).IsOptional().HasColumnType("datetime2");
            Property(b => b.InsurerMemo).IsOptional().HasMaxLength(512);
            Property(b => b.PolicyHolder).IsOptional().HasMaxLength(64);
            Property(b => b.PolicyHolderDate).IsOptional().HasColumnType("datetime2");
            Property(b => b.PolicySeal).IsOptional();
            Property(b => b.TransferVoucher).IsOptional().HasMaxLength(128);
        }
    }
}
