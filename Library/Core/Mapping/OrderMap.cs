using Domain.Orders;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Orders");
            HasKey(o => o.Id);
            HasMany(o => o.orderItem).WithRequired(pp => pp.order).HasForeignKey(pp => pp.order_Id);         
            HasMany(o => o.orderBatch).WithRequired(b => b.order).HasForeignKey(b => b.order_Id);
            Property(o => o.AgeRange).IsRequired().HasMaxLength(64);
            Property(o => o.AnnualExpense).IsRequired();
            Property(o => o.Author).IsRequired().HasMaxLength(256);
            Property(o => o.CommissionType).IsOptional().HasMaxLength(64);
            Property(o => o.CreateTime).IsRequired();
            Property(o => o.FanBao).IsOptional();
            Property(o => o.Memo).IsOptional().HasMaxLength(512);
            Property(o => o.Name).IsRequired().HasMaxLength(128);
            Property(o => o.Pretium).IsRequired();
            Property(o => o.Rebate).IsOptional();
            Property(o => o.StaffRange).IsRequired().HasMaxLength(50);
            Property(o => o.TiYong).IsOptional();
            Property(o => o.Changer).IsOptional().HasMaxLength(256);
            Property(o => o.ChangeDate).IsOptional();
            Property(o => o.StartDate).IsOptional().HasColumnType("datetime2");
            Property(o => o.EndDate).IsOptional().HasColumnType("datetime2");
            Property(o => o.CompanyName).IsOptional().HasMaxLength(128);
            Property(o => o.Linkman).IsOptional().HasMaxLength(32);
            Property(o => o.PhoneNumber).IsOptional().HasMaxLength(64);
            Property(o => o.Address).IsOptional().HasMaxLength(128);
            Property(o => o.BusinessLicense).IsOptional();
            Property(o => o.ProposalNo).IsOptional().HasMaxLength(32);
            Property(o => o.Insurer).IsOptional().HasMaxLength(32);
            Property(o => o.PolicyNumber).IsOptional().HasMaxLength(32);
            Property(o => o.ConfirmedDate).IsOptional().HasColumnType("datetime2");
        }
    }
}
