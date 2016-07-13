using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class CarInsuranceFileMap : EntityTypeConfiguration<CarInsuranceFile>
    {
        public CarInsuranceFileMap()
        {
            Property(s => s.Name).IsRequired().HasMaxLength(64);
            Property(s => s.Path).IsRequired().HasMaxLength(256);
            Property(s => s.Memo).IsOptional().HasMaxLength(512);
            Property(s => s.Url).IsOptional().HasMaxLength(256);
            Property(s => s.Author).IsRequired().HasMaxLength(64);
        }
    }
    //public class CarInsuranceEinsuranceMap : EntityTypeConfiguration<CarInsuranceEinsurance>
    //{
    //    public CarInsuranceEinsuranceMap()
    //    {
    //        Property(s => s.Name).IsRequired().HasMaxLength(64);
    //        Property(s => s.Path).IsRequired().HasMaxLength(256);
    //        Property(s => s.Memo).IsOptional().HasMaxLength(512);
    //        Property(s => s.Url).IsOptional().HasMaxLength(256);
    //        Property(s => s.Author).IsRequired().HasMaxLength(64);
    //    }
    //}
}
