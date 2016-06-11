namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentMenuId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.aspnet_MembershipAudit",
               c => new
               {
                   aspnet_MemberShipAuditId = c.Int(nullable: false, identity: true),
                   EmailId = c.String(nullable:false,unicode:true),
                   CreatedBy = c.Int(nullable: false),
                   LastLoginDate = c.DateTime(nullable: false),
                   LastLockOutDate = c.DateTime(),
                   CreatedDate = c.DateTime(nullable: false),
               })
               .PrimaryKey(t => t.aspnet_MemberShipAuditId);
            
            
            AddColumn("dbo.Menu", "MenuType", c => c.Int(nullable: false));
            AddColumn("dbo.Menu", "ParentMenuId", c => c.Int(nullable: false));
           // AddColumn("dbo.Menu", "ControllerName", c => c.String(nullable: false));
            AddColumn("dbo.Menu", "ActionName", c => c.String(nullable: false));
          //  CreateIndex("dbo.aspnet_Membership", "Emailid", true);
            RenameTable("Value", "ValueType");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menu", "ParentMenuId");
            DropColumn("dbo.aspnet_MembershipAudit", "Customer_CustomerId");
            DropColumn("dbo.Menu", "MenuType");
          //  DropColumn("dbo.Menu", "ControllerName");
            DropColumn("dbo.Menu", "ActionName");
            RenameTable("Value", "ValueType");
          //  DropIndex("dbo.aspnet_Membership", "Emailid");
            DropTable("dbo.aspnet_MembershipAudit");
        }
    }
}
