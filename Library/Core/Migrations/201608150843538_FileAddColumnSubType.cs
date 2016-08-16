namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileAddColumnSubType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseFile", "SubType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseFile", "SubType");
        }
    }
}
