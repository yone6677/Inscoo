using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class CarInsuranceDetailMap : EntityTypeConfiguration<CarInsuranceDetail>
    {
        public CarInsuranceDetailMap()
        {
            HasKey(c => c.DetailID);

            Ignore(c => c.Id);
            Ignore(c => c.Author);
            Ignore(c => c.CreateTime);
            Ignore(c => c.IsDeleted);

            Property(p => p.CarInsuranceID).IsOptional();
            Property(p => p.InsuredSetNumber).IsOptional();
            Property(p => p.InsuredExpense).IsOptional();
            Property(p => p.IsDelete).IsOptional();

            Property(p => p.InsuredDoc).HasMaxLength(200);
            Property(p => p.InsuredName).HasMaxLength(100);
            Property(p => p.InsuredCarNo).HasMaxLength(50);
            Property(p => p.InsuredCarID).HasMaxLength(50);
            Property(p => p.InsuredCompanyID).HasMaxLength(50);
            Property(p => p.InsuredTel).HasMaxLength(50);
            Property(p => p.InsuredMedicalFee).HasMaxLength(500);
            Property(p => p.InsurancePolicy).HasMaxLength(50);

            Property(p => p.InsuredBeginDate).IsOptional();
            Property(p => p.InsuredEndingDate).IsOptional();
            Property(p => p.InsuredInsertDate).IsOptional();
        }
    }
}
