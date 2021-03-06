﻿using Domain.Orders;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            ToTable("OrderItem");
            Property(o => o.Author).IsRequired().HasMaxLength(256);
            Property(o => o.CommissionRate).IsOptional();
            Property(o => o.CoverageSum).IsRequired().HasMaxLength(32);
            Property(o => o.InsuredWho).IsRequired().HasMaxLength(32);          
            Property(o => o.OriginalPrice).IsRequired();
            Property(o => o.PayoutRatio).IsRequired().HasMaxLength(32);
            Property(o => o.Price).IsRequired();
            Property(o => o.ProdType).IsRequired().HasMaxLength(32);
            Property(o => o.SafeguardCode).IsRequired().HasMaxLength(32);
            Property(o => o.SafeguardName).IsRequired().HasMaxLength(32);
            Property(o => o.ProdTimeLimit).IsOptional().HasMaxLength(10);
            Property(o => o.ProdAbatement).IsOptional().HasMaxLength(50);
            Property(o => o.ItemNo).IsOptional().HasMaxLength(10);
        }
    }
}
