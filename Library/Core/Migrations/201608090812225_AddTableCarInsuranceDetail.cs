namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableCarInsuranceDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarInsuranceDetail",
                c => new
                    {
                        DetailID = c.Int(nullable: false, identity: true),
                        CarInsuranceID = c.Int(),
                        InsuredDoc = c.String(maxLength: 200),
                        InsuredName = c.String(maxLength: 100),
                        InsuredCarNo = c.String(maxLength: 50),
                        InsuredCarID = c.String(maxLength: 50),
                        InsuredCompanyID = c.String(maxLength: 50),
                        InsuredTel = c.String(maxLength: 50),
                        InsuredSetNumber = c.Int(),
                        InsuredExpense = c.Int(),
                        InsuredBeginDate = c.DateTime(),
                        InsuredMedicalFee = c.String(maxLength: 500),
                        InsurancePolicy = c.String(maxLength: 50),
                        InsuredEndingDate = c.DateTime(),
                        InsuredInsertDate = c.DateTime(),
                        IsDelete = c.Int(),
                    })
                .PrimaryKey(t => t.DetailID);
            
            AddColumn("dbo.CarInsurance", "FileType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarInsurance", "FileType");
            DropTable("dbo.CarInsuranceDetail");
        }
    }
}
