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
            AlterColumn("dbo.Navigation", "name", c => c.String(nullable: false, maxLength: 16));
            DropTable("dbo.FileInfo");
            CreateIndex("dbo.BaseFile", "AppUserId");
            AddForeignKey("dbo.BaseFile", "AppUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
