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
            DropTable("dbo.FileInfo");
            DropTable("dbo.CarInsurance");
            CreateIndex("dbo.BaseFile", "AppUserId");
            AddForeignKey("dbo.BaseFile", "AppUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
