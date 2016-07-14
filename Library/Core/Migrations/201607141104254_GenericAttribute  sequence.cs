namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenericAttributesequence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GenericAttribute", "Sequence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GenericAttribute", "Sequence");
        }
    }
}
