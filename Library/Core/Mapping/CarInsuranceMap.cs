using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class CarInsuranceMap : EntityTypeConfiguration<CarInsurance>
    {
        public CarInsuranceMap()
        {
            HasRequired(c => c.Excel).WithMany().HasForeignKey(c => c.ExcelId);
            HasOptional(c => c.Einsurance).WithMany().HasForeignKey(c => c.EinsuranceId);
        }
    }
}
