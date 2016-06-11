namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aspnet_MembershipAudit : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.aspnet_MembershipAudit",
            //    c => new
            //        {
            //            aspnet_MemberShipAuditId = c.Int(nullable: false, identity: true),
            //            EmailId = c.String(),
            //            CreatedBy = c.Int(nullable: false),
            //            LastLoginDate = c.DateTime(nullable: false),
            //            LastLockOutDate = c.DateTime(),
            //            CreatedDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.aspnet_MemberShipAuditId);
            
            //AlterColumn("dbo.Menu", "Name", c => c.String(nullable: false, maxLength: 500));
            //AlterColumn("dbo.Menu", "ControllerName", c => c.String(nullable: false));
            //AlterColumn("dbo.Menu", "ActionName", c => c.String(nullable: false));
            //AlterColumn("dbo.Value", "ValueTypeName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Value", "ValueTypeName", c => c.String());
            //AlterColumn("dbo.Menu", "ActionName", c => c.String());
            //AlterColumn("dbo.Menu", "ControllerName", c => c.String());
            //AlterColumn("dbo.Menu", "Name", c => c.String(maxLength: 500));
            //DropTable("dbo.aspnet_MembershipAudit");
        }
    }
}
