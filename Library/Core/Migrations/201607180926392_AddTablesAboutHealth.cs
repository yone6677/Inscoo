namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablesAboutHealth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HealthOrderMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HealthCheckProductId = c.Int(nullable: false),
                        PublicPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionRatio = c.Int(nullable: false),
                        CommissionMethod = c.String(),
                        CheckBeginTime = c.DateTime(),
                        CheckEndTime = c.DateTime(),
                        CompanyId = c.Int(),
                        BaokuOrderCode = c.String(),
                        Status = c.Int(nullable: false),
                        PersonExcelPath = c.String(),
                        BaokuConfirmer = c.String(),
                        BaokuConfirmDate = c.DateTime(),
                        FinanceConfirmer = c.String(),
                        FinanceConfirmDate = c.DateTime(),
                        FinancePayDate = c.DateTime(),
                        FinanceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinanceBankSerialNumber = c.String(),
                        FinanceMemo = c.String(),
                        CheckComConfirmer = c.String(),
                        CheckComOrderCode = c.String(),
                        CheckComConfirmDate = c.DateTime(),
                        ExpressCom = c.String(),
                        ExpressCode = c.String(),
                        CheckComMemo = c.String(),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.HealthCheckProduct", t => t.HealthCheckProductId, cascadeDelete: true)
                .Index(t => t.HealthCheckProductId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.HealthCheckProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductType = c.String(),
                        ProductCode = c.String(),
                        ProductName = c.String(),
                        ProductMemo = c.String(),
                        CompanyCode = c.String(),
                        CompanyName = c.String(),
                        PublicPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BdPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChannelPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HrPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OtherPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionMethod = c.String(),
                        CheckProductPic = c.String(),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HealthOrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HealthOrderMasterId = c.Int(nullable: false),
                        Name = c.String(),
                        Sex = c.Boolean(nullable: false),
                        Birthday = c.DateTime(),
                        IdNumber = c.String(),
                        Marriage = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        CompanyName = c.String(),
                        DepartMent = c.String(),
                        Chair = c.String(),
                        OrderAccount = c.String(),
                        OrderPassword = c.String(),
                        ProcessDate = c.String(),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HealthOrderMaster", t => t.HealthOrderMasterId, cascadeDelete: true)
                .Index(t => t.HealthOrderMasterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HealthOrderDetail", "HealthOrderMasterId", "dbo.HealthOrderMaster");
            DropForeignKey("dbo.HealthOrderMaster", "HealthCheckProductId", "dbo.HealthCheckProduct");
            DropForeignKey("dbo.HealthOrderMaster", "CompanyId", "dbo.Company");
            DropIndex("dbo.HealthOrderDetail", new[] { "HealthOrderMasterId" });
            DropIndex("dbo.HealthOrderMaster", new[] { "CompanyId" });
            DropIndex("dbo.HealthOrderMaster", new[] { "HealthCheckProductId" });
            DropTable("dbo.HealthOrderDetail");
            DropTable("dbo.HealthCheckProduct");
            DropTable("dbo.HealthOrderMaster");
        }
    }
}
