namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class financeDateils : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CashFlowDetails", "TransferVoucher", c => c.String(maxLength: 256));
            AddColumn("dbo.CashFlowDetails", "Memo", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CashFlowDetails", "Memo");
            DropColumn("dbo.CashFlowDetails", "TransferVoucher");
        }
    }
}
