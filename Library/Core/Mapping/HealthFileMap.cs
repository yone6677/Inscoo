using Domain;
using System.Data.Entity.ModelConfiguration;


namespace Core.Mapping
{
    public class HealthFileMap : EntityTypeConfiguration<HealthFile>
    {
        public HealthFileMap()
        {
            ToTable("HealthFile");
            Property(f => f.CreateTime).HasColumnType("datetime2");
        }
    }
}
