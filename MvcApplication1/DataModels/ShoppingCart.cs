using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using MvcApplication1.Servicelocator;

namespace MvcApplication1.DataModels
{
    public class ShoppingCart : DbContext
    {
        
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnection"].ToString();

            }
        }

        public ShoppingCart()
            : base(ConfigurationManager.ConnectionStrings["DbConnection"].ToString())
        {
        }

        public ShoppingCart(string connectionString)
            : base(connectionString)
        {
            connectionString = this.ConnectionString;
        }

        public ShoppingCart(DbConnection connection)
            : base(connection, contextOwnsConnection: true)
        {
            ((IObjectContextAdapter)this).ObjectContext.Connection.Open();
        }

        public DbSet<CustomerModels> Customer { get; set; }
        public DbSet<EntityAddressModels> EntityAddress { get; set; }
        public DbSet<MerchantModels> Merchant { get; set; }
        public DbSet<aspnet_users> aspnet_User { get; set; }
        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public DbSet<aspnet_Role> aspnet_Role { get; set; }
        public DbSet<aspnet_UsersInRoles> aspnet_UserInRoles { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<ValueTypeModels> ValueType { get; set; }
        public DbSet<ValueTypeGroupModels> ValueTypeGroup { get; set; }
        public DbSet<ExternalLogin> ExternalLogin { get; set; }
        public DbSet<aspnet_MembershipAudit> aspnet_MembershipAudit { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<EntitySequence> EntitySequence { get; set; }

        public DbSet<aspnet_MenuPermission> aspnet_MenuPermission { get; set; }
    }
}