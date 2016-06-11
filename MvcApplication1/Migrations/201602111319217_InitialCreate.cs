namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Addressid = c.Int(nullable: false, identity: true),
                        AddressLine = c.String(nullable: false),
                        AddressLine2 = c.String(),
                        EntityId = c.Int(nullable: false),
                        AddressTypeId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        Pincode = c.Int(nullable: false),
                        Emailid = c.String(),
                        Mobile = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Addressid)
                .ForeignKey("Customer.Customer", t => t.EntityId, cascadeDelete: true)
                .ForeignKey("dbo.Merchant", t => t.EntityId, cascadeDelete: true);
               // .Index(t => t.EntityId)
                //.Index(t => t.EntityId);
            
            CreateTable(
                "dbo.Merchant",
                c => new
                    {
                        MerchantId = c.Int(nullable: false, identity: true),
                        MerchantCode = c.Long(nullable: false),
                        MerchantName = c.String(nullable: false),
                        Title = c.String(),
                        locationId = c.Int(nullable: false),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MerchantId);
            
            CreateTable(
                "dbo.aspnet_users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        NumericUserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 200),
                        LowerUserName = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.aspnet_UsersInRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.aspnet_users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.aspnet_Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.aspnet_Role",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        RoleName = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.aspnet_Membership",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Password = c.String(nullable: false),
                        PasswordFormat = c.Int(nullable: false),
                        PasswordSalt = c.String(),
                        isLive = c.Boolean(nullable: false),
                        IslockedOut = c.Boolean(nullable: false),
                        Emailid = c.String(nullable: false),
                        SecurityQuestion = c.String(),
                        SecurityAns = c.String(),
                        FailedPasswordAttemtptCount = c.Int(),
                        LastLockedDate = c.DateTime(),
                        LastLoginDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.aspnet_users", t => t.UserId)
                .Index(t => t.UserId).Index(t =>t.Emailid);
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        MenuId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                        Description = c.String(maxLength: 500),
                        Order = c.Int(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.Value",
                c => new
                    {
                        ValueTypeId = c.Int(nullable: false, identity: true),
                        ValueTypeName = c.String(),
                        ValueTypeGroupId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ValueTypeId)
                .ForeignKey("dbo.ValueTypeGroup", t => t.ValueTypeGroupId, cascadeDelete: true)
                .Index(t => t.ValueTypeGroupId);
            
            CreateTable(
                "dbo.ValueTypeGroup",
                c => new
                    {
                        ValueTypeGroupId = c.Int(nullable: false, identity: true),
                        ValueTypeGroupName = c.String(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CaretedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ValueTypeGroupId);
            
            CreateTable(
                "dbo.ExternalLogins",
                c => new
                    {
                        ExternalLoginId = c.Int(nullable: false, identity: true),
                        Provider = c.String(),
                        ProviderDisplayName = c.String(),
                        ProviderUserId = c.String(),
                    })
                .PrimaryKey(t => t.ExternalLoginId);
            
            CreateTable(
                "Customer.Customer",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        CustomerCode = c.Long(nullable: false),
                        CustomerName = c.String(nullable: false),
                        Title = c.String(),
                        locationId = c.Int(nullable: false),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CustomerId);

           
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Value", new[] { "ValueTypeGroupId" });
            DropIndex("dbo.aspnet_Membership", new[] { "UserId" });
            DropIndex("dbo.aspnet_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.aspnet_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.Address", new[] { "EntityId" });
            DropIndex("dbo.Address", new[] { "EntityId" });
            DropForeignKey("dbo.Value", "ValueTypeGroupId", "dbo.ValueTypeGroup");
            DropForeignKey("dbo.aspnet_Membership", "UserId", "dbo.aspnet_users");
            DropForeignKey("dbo.aspnet_UsersInRoles", "RoleId", "dbo.aspnet_Role");
            DropForeignKey("dbo.aspnet_UsersInRoles", "UserId", "dbo.aspnet_users");
            DropForeignKey("dbo.Address", "EntityId", "dbo.Merchant");
            DropForeignKey("dbo.Address", "EntityId", "Customer.Customer");
            DropTable("Customer.Customer");
            DropTable("dbo.ExternalLogins");
            DropTable("dbo.ValueTypeGroup");
            DropTable("dbo.Value");
            DropTable("dbo.Menu");
            DropTable("dbo.aspnet_Membership");
            DropTable("dbo.aspnet_Role");
            DropTable("dbo.aspnet_UsersInRoles");
            DropTable("dbo.aspnet_users");
            DropTable("dbo.Merchant");
            DropTable("dbo.Address");
           
        }
    }
}
