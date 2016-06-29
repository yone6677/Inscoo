﻿using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            ToTable("Permissions");
            Property(p => p.roleId).IsRequired().HasMaxLength(256);
        }
    }
}
