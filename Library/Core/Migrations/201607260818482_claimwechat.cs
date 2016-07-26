namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class claimwechat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClaimFromWechat", "RecipientEmail", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClaimFromWechat", "RecipientEmail", c => c.String(maxLength: 16));
        }
    }
}
