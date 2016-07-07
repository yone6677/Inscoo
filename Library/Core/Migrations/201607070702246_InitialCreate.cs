namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsDelete = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        CreaterId = c.String(maxLength: 128),
                        Changer = c.String(maxLength: 128),
                        CompanyName = c.String(maxLength: 128),
                        LinkMan = c.String(maxLength: 32),
                        TiYong = c.Boolean(nullable: false),
                        FanBao = c.Boolean(nullable: false),
                        Ident = c.Int(nullable: false, identity: true),
                        Rebate = c.Int(nullable: false),
                        AccountName = c.String(),
                        BankName = c.String(maxLength: 50),
                        BankNumber = c.String(maxLength: 50),
                        CommissionMethod = c.String(),
                        PortraitPath = c.String(),
                        ProdInsurance = c.String(),
                        ProdSeries = c.String(),
                        Email = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BaseFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Path = c.String(nullable: false, maxLength: 256),
                        Memo = c.String(maxLength: 512),
                        Url = c.String(maxLength: 256),
                        Author = c.String(nullable: false, maxLength: 64),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Type = c.String(maxLength: 64),
                        pId = c.Int(),
                        CompanyId = c.Int(),
                        AppUserId = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
                .Index(t => t.CompanyId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Address = c.String(nullable: false, maxLength: 100),
                        LinkMan = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 30),
                        Email = c.String(maxLength: 100),
                        UserId = c.String(nullable: false, maxLength: 128),
                        EditTime = c.DateTime(nullable: false),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Code, unique: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GenericAttribute",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KeyGroup = c.String(nullable: false, maxLength: 20),
                        Key = c.String(nullable: false, maxLength: 20),
                        Value = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Navigation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 16),
                        level = c.Int(nullable: false),
                        controller = c.String(maxLength: 64),
                        action = c.String(maxLength: 64),
                        url = c.String(maxLength: 128),
                        pId = c.Int(),
                        isShow = c.Boolean(nullable: false),
                        memo = c.String(maxLength: 256),
                        htmlAtt = c.String(maxLength: 256),
                        sequence = c.Int(),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderBatch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        order_Id = c.Int(nullable: false),
                        BNum = c.String(maxLength: 32),
                        BState = c.Int(),
                        EmpInfoFile = c.Int(),
                        EmpInfoFilePDF = c.Int(),
                        EmpInfoFileSeal = c.Int(),
                        PolicySeal = c.Int(),
                        PaymentNoticePDF = c.Int(),
                        PolicyHolder = c.String(maxLength: 64),
                        PolicyHolderDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        InscooConfirm = c.String(maxLength: 64),
                        InscooConfirmDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Finance = c.String(maxLength: 64),
                        FinanceDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        AmountCollected = c.Decimal(precision: 18, scale: 2),
                        CollectionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        TransferVoucher = c.String(maxLength: 128),
                        FinanceMemo = c.String(maxLength: 512),
                        Insurer = c.String(maxLength: 64),
                        InsurerConfirmDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CourierNumber = c.String(maxLength: 32),
                        InsurerMemo = c.String(maxLength: 512),
                        Author = c.String(maxLength: 64),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.order_Id, cascadeDelete: true)
                .Index(t => t.order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        Memo = c.String(maxLength: 512),
                        StaffRange = c.String(nullable: false, maxLength: 50),
                        AgeRange = c.String(nullable: false, maxLength: 64),
                        AnnualExpense = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pretium = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TiYong = c.Int(),
                        FanBao = c.Int(),
                        Rebate = c.Int(),
                        CommissionType = c.String(maxLength: 64),
                        State = c.Int(nullable: false),
                        Changer = c.String(maxLength: 256),
                        ChangeDate = c.DateTime(),
                        StartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CompanyName = c.String(maxLength: 128),
                        Linkman = c.String(maxLength: 32),
                        PhoneNumber = c.String(maxLength: 64),
                        Address = c.String(maxLength: 128),
                        BusinessLicense = c.Int(),
                        ProposalNo = c.String(maxLength: 32),
                        Insurer = c.String(maxLength: 32),
                        PolicyNumber = c.String(maxLength: 32),
                        ConfirmedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        InsuranceNumber = c.Int(nullable: false),
                        Author = c.String(nullable: false, maxLength: 256),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProdType = c.String(nullable: false, maxLength: 32),
                        SafeguardName = c.String(nullable: false, maxLength: 32),
                        SafeguardCode = c.String(nullable: false, maxLength: 32),
                        InsuredWho = c.String(nullable: false, maxLength: 32),
                        CoverageSum = c.String(nullable: false, maxLength: 32),
                        PayoutRatio = c.String(nullable: false, maxLength: 32),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionRate = c.Single(),
                        order_Id = c.Int(nullable: false),
                        Author = c.String(nullable: false, maxLength: 256),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.order_Id, cascadeDelete: true)
                .Index(t => t.order_Id);
            
            CreateTable(
                "dbo.OrderEmployees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        batch_Id = c.Int(nullable: false),
                        Author = c.String(maxLength: 64),
                        CreateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderBatch", t => t.batch_Id, cascadeDelete: true)
                .Index(t => t.batch_Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        roleId = c.String(nullable: false, maxLength: 256),
                        NavigationId = c.Int(nullable: false),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Navigation", t => t.NavigationId, cascadeDelete: true)
                .Index(t => t.NavigationId);
            
            CreateTable(
                "dbo.ProductMixItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mid = c.Int(nullable: false),
                        SafefuardName = c.String(nullable: false, maxLength: 50),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CoverageSum = c.String(nullable: false, maxLength: 50),
                        PayoutRatio = c.String(maxLength: 50),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductMix", t => t.mid, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.product_Id)
                .Index(t => t.mid)
                .Index(t => t.product_Id);
            
            CreateTable(
                "dbo.ProductMix",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        Description = c.String(maxLength: 256),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address = c.String(nullable: false, maxLength: 50),
                        StaffRange = c.String(nullable: false, maxLength: 50),
                        AgeRange = c.String(nullable: false, maxLength: 50),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemNo = c.String(maxLength: 10),
                        ProdType = c.String(maxLength: 20),
                        SafeguardCode = c.String(maxLength: 20),
                        SafeguardName = c.String(maxLength: 50),
                        InsuredWho = c.String(maxLength: 10),
                        CoverageSum = c.String(maxLength: 10),
                        PayoutRatio = c.String(maxLength: 20),
                        HeadCount3 = c.String(maxLength: 20),
                        HeadCount5 = c.String(maxLength: 20),
                        HeadCount11 = c.String(maxLength: 20),
                        HeadCount31 = c.String(maxLength: 20),
                        HeadCount51 = c.String(maxLength: 20),
                        HeadCount100 = c.String(maxLength: 20),
                        InsuredCom = c.String(maxLength: 20),
                        ClaimCode = c.String(maxLength: 5),
                        CommissionRate = c.Single(),
                        changer = c.String(),
                        ProvisionPath = c.String(),
                        Author = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductMixItem", "product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductMixItem", "mid", "dbo.ProductMix");
            DropForeignKey("dbo.Permissions", "NavigationId", "dbo.Navigation");
            DropForeignKey("dbo.OrderEmployees", "batch_Id", "dbo.OrderBatch");
            DropForeignKey("dbo.OrderItem", "order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderBatch", "order_Id", "dbo.Orders");
            DropForeignKey("dbo.BaseFile", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Company", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BaseFile", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.ProductMixItem", new[] { "product_Id" });
            DropIndex("dbo.ProductMixItem", new[] { "mid" });
            DropIndex("dbo.Permissions", new[] { "NavigationId" });
            DropIndex("dbo.OrderEmployees", new[] { "batch_Id" });
            DropIndex("dbo.OrderItem", new[] { "order_Id" });
            DropIndex("dbo.OrderBatch", new[] { "order_Id" });
            DropIndex("dbo.Company", new[] { "UserId" });
            DropIndex("dbo.Company", new[] { "Code" });
            DropIndex("dbo.BaseFile", new[] { "AppUserId" });
            DropIndex("dbo.BaseFile", new[] { "CompanyId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.Products");
            DropTable("dbo.ProductMix");
            DropTable("dbo.ProductMixItem");
            DropTable("dbo.Permissions");
            DropTable("dbo.OrderEmployees");
            DropTable("dbo.OrderItem");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderBatch");
            DropTable("dbo.Navigation");
            DropTable("dbo.GenericAttribute");
            DropTable("dbo.Company");
            DropTable("dbo.BaseFile");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
