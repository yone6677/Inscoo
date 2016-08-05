namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompany_AlterCodeLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Company", new[] { "Code" });
            AlterColumn("dbo.Company", "Code", c => c.String(nullable: false, maxLength: 30, unicode: false));
            CreateIndex("dbo.Company", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Company", new[] { "Code" });
            AlterColumn("dbo.Company", "Code", c => c.String(nullable: false, maxLength: 10, unicode: false));
            CreateIndex("dbo.Company", "Code", unique: true);
        }
    }
}
