namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class express : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderBatch", "Express", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderBatch", "Express");
        }
    }
}
