namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddColumnMemo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Memo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Memo");
        }
    }
}
