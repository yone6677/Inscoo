namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class healthEmployeesTemp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HealthEmpTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HealthOrderMasterId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Sex = c.Boolean(nullable: false),
                        Birthday = c.DateTime(precision: 7, storeType: "datetime2"),
                        IdNumber = c.String(nullable: false, maxLength: 20),
                        Marriage = c.String(),
                        Phone = c.String(nullable: false, maxLength: 16),
                        Email = c.String(),
                        Address = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(),
                        DepartMent = c.String(),
                        Chair = c.String(),
                        OrderAccount = c.String(),
                        OrderPassword = c.String(),
                        ProcessDate = c.String(),
                        Ticks = c.Long(nullable: false),
                        Author = c.String(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HealthOrderMaster", t => t.HealthOrderMasterId, cascadeDelete: true)
                .Index(t => t.HealthOrderMasterId);
            
            DropColumn("dbo.AspNetUsers", "Memo");
            DropColumn("dbo.BaseFile", "SubType");
            DropTable("dbo.ClaimFilesList");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClaimFilesList",
                c => new
                    {
                        ClaimFilesListID = c.Int(nullable: false, identity: true),
                        ClaimFilesBatchCode = c.String(nullable: false, maxLength: 50),
                        ClaimFilesName = c.String(nullable: false, maxLength: 50),
                        ClaimFilesStatus = c.String(nullable: false, maxLength: 1),
                        ClaimFilesCreateID = c.String(nullable: false),
                        ClaimFilesCreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClaimFilesListID);
            
            AddColumn("dbo.BaseFile", "SubType", c => c.String());
            AddColumn("dbo.AspNetUsers", "Memo", c => c.String());
            DropForeignKey("dbo.HealthEmpTemp", "HealthOrderMasterId", "dbo.HealthOrderMaster");
            DropIndex("dbo.HealthEmpTemp", new[] { "HealthOrderMasterId" });
            DropTable("dbo.HealthEmpTemp");
        }
    }
}
