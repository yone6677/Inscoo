namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyPDF : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderBatch", "PolicyPDF", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderBatch", "PolicyPDF");
        }
    }
}
