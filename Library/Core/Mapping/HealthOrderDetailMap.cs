﻿using System.Data.Entity.ModelConfiguration;
using Domain;

namespace Core.Mapping
{
    public class HealthOrderDetailMap : EntityTypeConfiguration<HealthOrderDetail>
    {
        public HealthOrderDetailMap()
        {
            Property(h => h.Birthday).IsOptional();
            Property(h => h.Author).IsRequired();
            HasRequired(h => h.HealthOrderMaster).WithMany(h => h.HealthOrderDetails).HasForeignKey(h => h.HealthOrderMasterId);
        }
    }
}
