namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cleanproductforeignkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductMixItem", "product_Id", "dbo.Products");
            DropIndex("dbo.ProductMixItem", new[] { "product_Id" });
            DropColumn("dbo.ProductMixItem", "product_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductMixItem", "product_Id", c => c.Int());
            CreateIndex("dbo.ProductMixItem", "product_Id");
            AddForeignKey("dbo.ProductMixItem", "product_Id", "dbo.Products", "Id");
        }
    }
}
