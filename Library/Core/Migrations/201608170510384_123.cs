namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.AspNetUsers", "Memo", c => c.String());
            AddColumn("dbo.BaseFile", "SubType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseFile", "SubType");
            DropColumn("dbo.AspNetUsers", "Memo");
            DropTable("dbo.ClaimFilesList");
        }
    }
}
