namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableMemberInsurance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberInsurance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueKey = c.String(maxLength: 50),
                        FileType = c.Int(nullable: false),
                        FileTypeName = c.String(nullable: false, maxLength: 50),
                        ExcelId = c.Int(nullable: false),
                        EinsuranceId = c.Int(),
                        EOrderCode = c.String(maxLength: 50),
                        Status = c.String(maxLength: 5),
                        PdfFileName = c.String(maxLength: 100),
                        Author = c.String(nullable: false, maxLength: 100),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileInfo", t => t.EinsuranceId)
                .ForeignKey("dbo.FileInfo", t => t.ExcelId, cascadeDelete: true)
                .Index(t => t.ExcelId)
                .Index(t => t.EinsuranceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberInsurance", "ExcelId", "dbo.FileInfo");
            DropForeignKey("dbo.MemberInsurance", "EinsuranceId", "dbo.FileInfo");
            DropIndex("dbo.MemberInsurance", new[] { "EinsuranceId" });
            DropIndex("dbo.MemberInsurance", new[] { "ExcelId" });
            DropTable("dbo.MemberInsurance");
        }
    }
}
