using Logger.Models;
using System.Data.Entity.ModelConfiguration;

namespace Logger.Mapping
{
    public class LogsMap: EntityTypeConfiguration<Logs>
    {
        public LogsMap()
        {
            ToTable("Logs");
            HasKey(l => l.Id);
            Property(l => l.Ip).IsOptional().HasMaxLength(64);
            Property(l => l.Level).IsOptional();
            Property(l => l.Browser).IsOptional().HasMaxLength(64);
            Property(l => l.CreateDate).IsOptional();
            Property(l => l.Memo).IsOptional().HasMaxLength(1024);
            Property(l => l.Message).IsOptional().HasMaxLength(1024);
            Property(l => l.Parameters).IsOptional().HasMaxLength(128);
            Property(l => l.Uid).IsOptional().HasMaxLength(64);
            Property(l => l.Url).IsOptional().HasMaxLength(128);
        }
    }
}