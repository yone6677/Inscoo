using System;
using Domain;
using System.Data.Entity.ModelConfiguration;

namespace Core.Mapping
{
    public class ClaimManagementDetailMap : EntityTypeConfiguration<ClaimManagementDetail>
    {
        public ClaimManagementDetailMap()
        {
            Property(p => p.ClaimBatch).IsOptional().HasMaxLength(20);
            Property(p => p.InsuranceGroup).IsOptional().HasMaxLength(50);
            Property(p => p.InsuranceGroupName).IsOptional().HasMaxLength(50);
            Property(p => p.InsuranceSubGroup).IsOptional().HasMaxLength(50);
            Property(p => p.InsuranceSubCustomerCode).IsOptional().HasMaxLength(50);
            Property(p => p.InsuranceSubCustomerName).IsOptional().HasMaxLength(100);
            Property(p => p.InsuranceNo).IsOptional().HasMaxLength(50);
            Property(p => p.InsuranceEffectiveDate).IsOptional();
            Property(p => p.InsuranceExpiryDate).IsOptional();
            Property(p => p.SpaceDay).IsOptional().HasMaxLength(10);
            Property(p => p.BeloneInsuranceName).IsOptional().HasMaxLength(30);
            Property(p => p.InsuranceName).IsOptional().HasMaxLength(30);
            Property(p => p.InsuranceRel).IsOptional().HasMaxLength(10);
            Property(p => p.InsuranceSex).IsOptional().HasMaxLength(10);
            Property(p => p.InsuranceManID).IsOptional().HasMaxLength(30);
            Property(p => p.InsuranceBirthDay).IsOptional().HasMaxLength(20);

            Property(p => p.InsuranceAge).IsOptional().HasMaxLength(10);
            Property(p => p.InsuranceSBflag).IsOptional().HasMaxLength(20);
            Property(p => p.InsuranceSBcity).IsOptional().HasMaxLength(20);
            Property(p => p.ClaimCaseID).IsOptional().HasMaxLength(20);
            Property(p => p.ClaimApplyDate).IsOptional();
            Property(p => p.ClaimOperDate).IsOptional();

            Property(p => p.ClaimPayDate).IsOptional();
            Property(p => p.ClaimAccdtDate).IsOptional();
            Property(p => p.ClaimDoctDate).IsOptional();
            Property(p => p.ClaimDoctFDate).IsOptional();
            Property(p => p.ClaimType).IsOptional().HasMaxLength(20);
            Property(p => p.ClaimInsName).IsOptional().HasMaxLength(20);
            Property(p => p.InsProdID).IsOptional().HasMaxLength(20);
            Property(p => p.HospitalCode).IsOptional().HasMaxLength(20);
            Property(p => p.HospitalName).IsOptional().HasMaxLength(50);
            Property(p => p.HospitalCity).IsOptional().HasMaxLength(20);

            Property(p => p.HospitalType).IsOptional().HasMaxLength(10);
            Property(p => p.HospitalLevel).IsOptional().HasMaxLength(10);
            Property(p => p.HospitalKind).IsOptional().HasMaxLength(10);
            Property(p => p.HospitalSpecial).IsOptional().HasMaxLength(10);
            Property(p => p.HospitalYBDD).IsOptional().HasMaxLength(10);
            Property(p => p.DiseaseCode).IsOptional().HasMaxLength(20);

            Property(p => p.DiseaseDisc).IsOptional().HasMaxLength(50);
            Property(p => p.TimesDay).IsOptional().HasMaxLength(10);
            Property(p => p.ClaimResult).IsOptional().HasMaxLength(100);
            Property(p => p.ExpTotal).IsOptional().HasMaxLength(20);
            Property(p => p.ExpTotalDrog).IsOptional().HasMaxLength(20);
            Property(p => p.PayFromAccount).IsOptional().HasMaxLength(20);
            Property(p => p.PayForGov).IsOptional().HasMaxLength(20);
            Property(p => p.PayKind).IsOptional().HasMaxLength(20);

            Property(p => p.PaySelf).IsOptional().HasMaxLength(20);
            Property(p => p.PayThiryPart).IsOptional().HasMaxLength(20);
            Property(p => p.ApplyAmt).IsOptional().HasMaxLength(20);
            Property(p => p.Deductible).IsOptional().HasMaxLength(20);
            Property(p => p.DeductibleTime).IsOptional().HasMaxLength(20);
            Property(p => p.ClaimSum).IsOptional().HasMaxLength(20);

            Property(p => p.ClaimAmt).IsOptional().HasMaxLength(20);
            Property(p => p.DebitAmt).IsOptional().HasMaxLength(20);
            Property(p => p.DebitDiscript).IsOptional().HasMaxLength(500);

            Ignore(c => c.Author);
            Ignore(c => c.CreateTime);
            Ignore(c => c.IsDeleted);
        }
    }
}
