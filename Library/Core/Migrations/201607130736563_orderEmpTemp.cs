namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderEmpTemp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderEmpTemp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Bid = c.Int(nullable: false),
                        BNum = c.String(),
                        PMCode = c.String(maxLength: 32),
                        PMName = c.String(maxLength: 32),
                        Relationship = c.String(maxLength: 16),
                        IDType = c.String(nullable: false, maxLength: 16),
                        IDNumber = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 32),
                        Sex = c.String(maxLength: 16),
                        BirBirthday = c.DateTime(nullable: false),
                        Premium = c.Decimal(precision: 18, scale: 2),
                        BankCard = c.String(maxLength: 64),
                        BankName = c.String(maxLength: 32),
                        PhoneNumber = c.String(maxLength: 16),
                        Email = c.String(maxLength: 32),
                        HasSocialSecurity = c.String(maxLength: 32),
                        StartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Author = c.String(maxLength: 64),
                        CreateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderEmpTemp");
        }
    }
}
