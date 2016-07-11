namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddTable_ClaimManagementDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClaimManagementDetail",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ClaimBatch = c.String(maxLength: 20),
                    InsuranceGroup = c.String(maxLength: 50),
                    InsuranceGroupName = c.String(maxLength: 50),
                    InsuranceSubGroup = c.String(maxLength: 50),
                    InsuranceSubCustomerCode = c.String(maxLength: 50),
                    InsuranceSubCustomerName = c.String(maxLength: 100),
                    InsuranceNo = c.String(maxLength: 50),
                    InsuranceEffectiveDate = c.DateTime(nullable: true),
                    InsuranceExpiryDate = c.DateTime(nullable: true),
                    SpaceDay = c.String(maxLength: 10),
                    BeloneInsuranceName = c.String(maxLength: 30),
                    InsuranceName = c.String(maxLength: 30),
                    InsuranceRel = c.String(maxLength: 10),
                    InsuranceSex = c.String(maxLength: 10),
                    InsuranceManID = c.String(maxLength: 30),
                    InsuranceBirthDay = c.String(maxLength: 20),
                    InsuranceAge = c.String(maxLength: 10),
                    InsuranceSBflag = c.String(maxLength: 20),
                    InsuranceSBcity = c.String(maxLength: 20),
                    ClaimCaseID = c.String(maxLength: 20),
                    ClaimApplyDate = c.DateTime(nullable: true),
                    ClaimOperDate = c.DateTime(nullable: true),
                    ClaimPayDate = c.DateTime(nullable: true),
                    ClaimAccdtDate = c.DateTime(nullable: true),
                    ClaimDoctDate = c.DateTime(nullable: true),
                    ClaimDoctFDate = c.DateTime(nullable: true),
                    ClaimType = c.String(maxLength: 20),
                    ClaimInsName = c.String(maxLength: 20),
                    InsProdID = c.String(maxLength: 20),
                    HospitalCode = c.String(maxLength: 20),
                    HospitalName = c.String(maxLength: 50),
                    HospitalCity = c.String(maxLength: 20),
                    HospitalType = c.String(maxLength: 10),
                    HospitalLevel = c.String(maxLength: 10),
                    HospitalKind = c.String(maxLength: 10),
                    HospitalSpecial = c.String(maxLength: 10),
                    HospitalYBDD = c.String(maxLength: 10),
                    DiseaseCode = c.String(maxLength: 20),
                    DiseaseDisc = c.String(maxLength: 50),
                    TimesDay = c.String(maxLength: 10),
                    ClaimResult = c.String(maxLength: 100),
                    ExpTotal = c.String(maxLength: 20),
                    ExpTotalDrog = c.String(maxLength: 20),
                    PayFromAccount = c.String(maxLength: 20),
                    PayForGov = c.String(maxLength: 20),
                    PayKind = c.String(maxLength: 20),
                    PaySelf = c.String(maxLength: 20),
                    PayThiryPart = c.String(maxLength: 20),
                    ApplyAmt = c.String(maxLength: 20),
                    Deductible = c.String(maxLength: 20),
                    DeductibleTime = c.String(maxLength: 20),
                    ClaimSum = c.String(maxLength: 20),
                    ClaimAmt = c.String(maxLength: 20),
                    DebitAmt = c.String(maxLength: 20),
                    DebitDiscript = c.String(maxLength: 500),
                    CreateDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.ClaimManagementDetail");
        }
    }
}
