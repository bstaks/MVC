using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MvcApplication1.DataModels;
using System.Configuration;

namespace MvcApplication1.Custom
{
    public class CustomMemebership : MembershipUser
    {
        private string userName;
        public CustomMemebership()
        {


        }
        public CustomMemebership(string userName)
        {
            this.userName = userName;

        }


        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            using (ShoppingCart dbContext = new ShoppingCart())
            {
                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, ConfigurationManager.AppSettings["PasswordFormat"].ToString());

                var aspnetUser = (from u in dbContext.aspnet_User
                                  join
                                  member in dbContext.aspnet_Membership on
                                  u.UserId equals member.UserId
                                  where u.LowerUserName ==
                                  this.userName.ToLower() && member.Password == password
                                  select member)
                                                                    .FirstOrDefault();
                if (aspnetUser != null) {
                    aspnetUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, ConfigurationManager.AppSettings["PasswordFormat"].ToString());
                    dbContext.SaveChanges();
                    return true;
                }
                   
                return false;
            }

        }
    }
}