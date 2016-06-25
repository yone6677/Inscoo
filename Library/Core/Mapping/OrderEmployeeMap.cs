using Domain.Orders;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class OrderEmployeeMap : EntityTypeConfiguration<OrderEmployee>
    {
        public OrderEmployeeMap()
        {
            ToTable("OrderEmployees");
            Property(o => o.Author).IsOptional().HasMaxLength(64);
            Property(o => o.BankCard).IsOptional().HasMaxLength(64);
            Property(o => o.BankName).IsOptional().HasMaxLength(32);
            Property(o => o.BirBirthday).IsRequired();
            Property(o => o.CreateTime).IsRequired().HasColumnType("datetime2");
            Property(o => o.Email).IsOptional().HasMaxLength(32);
            Property(o => o.EndDate).IsOptional().HasColumnType("datetime2");
            Property(o => o.HasSocialSecurity).IsOptional().HasMaxLength(32);
            Property(o => o.IDNumber).IsRequired().HasMaxLength(32);
            Property(o => o.IDType).IsRequired().HasMaxLength(16);
            Property(o => o.Name).IsRequired().HasMaxLength(32);
            Property(o => o.PhoneNumber).IsOptional().HasMaxLength(16);
            Property(o => o.PMCode).IsOptional().HasMaxLength(32);
            Property(o => o.PMName).IsOptional().HasMaxLength(32);
            Property(o => o.Premium).IsOptional();
            Property(o => o.Relationship).IsOptional().HasMaxLength(16);
            Property(o => o.Sex).IsOptional().HasMaxLength(16);
            Property(o => o.StartDate).IsOptional().HasColumnType("datetime2");
        }
    }
}
