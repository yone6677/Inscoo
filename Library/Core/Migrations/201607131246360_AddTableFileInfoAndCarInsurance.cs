namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableFileInfoAndCarInsurance : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BaseFile", "AppUserId", "dbo.AspNetUsers");
            DropIndex("dbo.BaseFile", new[] { "AppUserId" });
            CreateTable(
                "dbo.CarInsurance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppUserId = c.String(maxLength: 128),
                        ExcelId = c.Int(nullable: false),
                        EinsuranceId = c.Int(),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
                .ForeignKey("dbo.FileInfo", t => t.EinsuranceId)
                .ForeignKey("dbo.FileInfo", t => t.ExcelId, cascadeDelete: true)
                .Index(t => t.AppUserId)
                .Index(t => t.ExcelId)
                .Index(t => t.EinsuranceId);
            
            CreateTable(
                "dbo.FileInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Path = c.String(nullable: false, maxLength: 256),
                        Memo = c.String(maxLength: 512),
                        Url = c.String(maxLength: 256),
                        CarInsuranceId = c.Int(nullable: false),
                        EditTime = c.DateTime(nullable: false),
                        Author = c.String(nullable: false, maxLength: 64),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderEmpTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Bid = c.Int(nullable: false),
                        BNum = c.String(),
                        BuyType = c.Int(nullable: false),
                        PMCode = c.String(maxLength: 32),
                        PMName = c.String(maxLength: 32),
                        Relationship = c.String(maxLength: 16),
                        IDType = c.String(nullable: false, maxLength: 16),
                        IDNumber = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 32),
                        Sex = c.String(maxLength: 16),
                        BirBirthday = c.DateTime(nullable: false),
                        Premium = c.Decimal(precision: 18, scale: 2),
                        BankCard = c.String(maxLength: 64),
                        BankName = c.String(maxLength: 32),
                        PhoneNumber = c.String(maxLength: 16),
                        Email = c.String(maxLength: 32),
                        HasSocialSecurity = c.String(maxLength: 32),
                        StartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Author = c.String(maxLength: 64),
                        CreateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "ProdInsuredName", c => c.String());
            AddColumn("dbo.Products", "ProdMemo", c => c.String());
            AlterColumn("dbo.Navigation", "name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.BaseFile", "AppUserId");
            DropColumn("dbo.BaseFile", "EinsuranceEditTime");
            DropColumn("dbo.BaseFile", "EinsurancePath");
            DropColumn("dbo.BaseFile", "EinsuranceUrl");
            DropColumn("dbo.BaseFile", "EinsuranceName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseFile", "EinsuranceName", c => c.String());
            AddColumn("dbo.BaseFile", "EinsuranceUrl", c => c.String());
            AddColumn("dbo.BaseFile", "EinsurancePath", c => c.String());
            AddColumn("dbo.BaseFile", "EinsuranceEditTime", c => c.DateTime());
            AddColumn("dbo.BaseFile", "AppUserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.CarInsurance", "ExcelId", "dbo.FileInfo");
            DropForeignKey("dbo.CarInsurance", "EinsuranceId", "dbo.FileInfo");
            DropForeignKey("dbo.CarInsurance", "AppUserId", "dbo.AspNetUsers");
            DropIndex("dbo.CarInsurance", new[] { "EinsuranceId" });
            DropIndex("dbo.CarInsurance", new[] { "ExcelId" });
            DropIndex("dbo.CarInsurance", new[] { "AppUserId" });
            AlterColumn("dbo.Navigation", "name", c => c.String(nullable: false, maxLength: 16));
            DropColumn("dbo.Products", "ProdMemo");
            DropColumn("dbo.Products", "ProdInsuredName");
            DropTable("dbo.OrderEmpTemp");
            DropTable("dbo.FileInfo");
            DropTable("dbo.CarInsurance");
            CreateIndex("dbo.BaseFile", "AppUserId");
            AddForeignKey("dbo.BaseFile", "AppUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
