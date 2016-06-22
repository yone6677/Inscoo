﻿using Domain.Orders;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Orders");
            HasMany(p => p.orderItem).WithRequired(pp => pp.order).HasForeignKey(pp => pp.OId);
            Property(o => o.AgeRange).IsRequired().HasMaxLength(64);
            Property(o => o.Amount).IsRequired();
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
        }
    }
}