namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wechatClaim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimFromWechat", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClaimFromWechat", "State");
        }
    }
}
