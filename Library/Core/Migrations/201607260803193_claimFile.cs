namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class claimFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.claimFileFromWechat", "fileType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.claimFileFromWechat", "fileType");
        }
    }
}
