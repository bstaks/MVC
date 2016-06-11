using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcApplication1.Filters;
using MvcApplication1.Models;
using MvcApplication1.DataModels;
using System.Configuration;
using MvcApplication1.Custom;
using System.Collections;
using System.Web.Caching;
using MvcApplication1.Servicelocator;

namespace MvcApplication1.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {

        private ShoppingCart ShoppingCart;

        public AccountController(ShoppingCart dbContex) 
        {
            ShoppingCart = dbContex ;
        }


        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewData["IsVisible"] = false;
            ViewBag.ReturnUrl = returnUrl;
            if (Request.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ViewData["IsVisible"] = false;
            using (ShoppingCart dbContext = new ShoppingCart())
            {
                if (ModelState.IsValid)
                {
                    string password = FormsAuthentication.
                                             HashPasswordForStoringInConfigFile(model.Password, ConfigurationManager.
                                             AppSettings["PasswordFormat"].ToString());
                    var aspnetMember = (from u in dbContext.aspnet_User
                                        join
                                        member in dbContext.aspnet_Membership on
                                        u.UserId equals member.UserId
                                        where u.LowerUserName ==
                                        model.UserName
                                        &&
                                        member.Password == password
                                        select member)
                                                                      .FirstOrDefault();


                    if (ModelState.IsValid && aspnetMember != null)
                    {
                        int entityType = 0;
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        aspnet_MembershipAudit aspnet_MembershipAudit = new DataModels.aspnet_MembershipAudit();
                        var entityId = (dbContext.aspnet_User.FirstOrDefault(m => m.UserId == aspnetMember.UserId).NumericUserId);
                        aspnet_MembershipAudit.CreatedBy = entityId;
                        ;
                        aspnet_MembershipAudit.CreatedDate = DateTime.Now;
                        aspnet_MembershipAudit.LastLockOutDate = aspnetMember.LastLockedDate.Equals("01/01/0001") ? null : aspnetMember.LastLockedDate;
                        aspnet_MembershipAudit.EmailId = aspnetMember.Emailid;
                        aspnet_MembershipAudit.LastLoginDate = aspnetMember.LastLoginDate == null ? null : aspnetMember.LastLoginDate;

                        aspnet_MembershipAudit.EntityId = GetEntityType(dbContext, entityId, out entityType);
                        aspnet_MembershipAudit.EntityType = entityType;
                        if (aspnetMember.LastLoginDate != null && aspnet_MembershipAudit.EntityId > 0)
                        {
                            aspnet_MembershipAudit.LastLoginIP = aspnetMember.LastLoginIP;
                            dbContext.aspnet_MembershipAudit.Add(aspnet_MembershipAudit);
                        }
                        aspnetMember.isLive = true;
                        aspnetMember.LastLoginIP = Request.ServerVariables["Remote_ADDR"];
                        aspnetMember.LastLoginDate = DateTime.Now;
                        dbContext.SaveChanges();
                        return RedirectToLocal(returnUrl);
                    }
                }
            }

            //if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        private int GetEntityType(ShoppingCart dbContext, int entityId, out int entityType)
        {
            var customer = (dbContext.Customer.FirstOrDefault(m => m.UserId == entityId));
            if (customer != null)
            {
                entityType = (int)EnumsType.EnumTypes.EntityType.Customer;
                return customer.CustomerId;
            }
            var merchant = (dbContext.Merchant.FirstOrDefault(m => m.UserId == entityId));
            if (merchant != null)
            {
                entityType = (int)EnumsType.EnumTypes.EntityType.Merchant;
                return merchant.MerchantId;
            }
            entityType = (int)EnumsType.EnumTypes.EntityType.Admin;
            return entityId;
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ClearApplicationCache();
            WebSecurity.Logout();

       //     OutputCacheAttribute.
            return RedirectToAction("Login", "Account");
        }

        public void ClearApplicationCache()
        {
            // clear cache child action
            foreach (var item in OutputCacheAttribute.ChildActionCache.AsEnumerable())
            {
                OutputCacheAttribute.ChildActionCache.Remove(item.Key);
            }


            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    using (ShoppingCart dbContext = new ShoppingCart())
                    {
                        var userNameCheck = dbContext.aspnet_User.FirstOrDefault(m => m.LowerUserName == model.UserName.ToLower());
                        if (userNameCheck != null)
                        {
                            ModelState.AddModelError("", "UserName already exits!");
                            ViewData["ExitsUserName"] = "UserName already exits!";
                            return View(model);
                        }


                        var emailCheck = dbContext.aspnet_Membership.FirstOrDefault(m => m.Emailid.ToLower() == model.Emailid.ToLower());
                        if (emailCheck != null)
                        {
                            ModelState.AddModelError("", "EmailId already registered");
                            return View(model);
                        }

                        userNameCheck = RegisterUser(model, dbContext, userNameCheck);
                        //     FormsAuthentication.SetAuthCookie(userNameCheck.UserName, true);
                        ModelState.AddModelError("", "Register successfully!");

                    }


                    return View();
                }
                //catch (MembershipCreateUserException e)
                //{
                //    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                //}
                catch { }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public static aspnet_users RegisterUser(RegisterModel model, ShoppingCart dbContext, aspnet_users userNameCheck)
        {
            userNameCheck = new aspnet_users();
            userNameCheck.UserId = Guid.NewGuid();
            userNameCheck.UserName = model.UserName;
            userNameCheck.LowerUserName = model.UserName.ToLower();
            userNameCheck.CreatedDate = DateTime.Now;

            dbContext.aspnet_User.Add(userNameCheck);
            dbContext.SaveChanges();

            aspnet_Membership membership = new aspnet_Membership();
            membership.UserId = userNameCheck.UserId;
            membership.Emailid = model.Emailid;
            membership.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, ConfigurationManager.AppSettings["PasswordFormat"].ToString());
            membership.PasswordFormat = Convert.ToInt16(ConfigurationManager.AppSettings["PasswordType"].ToString());
            membership.IslockedOut = false;
            //   membership.LastLoginDate = DateTime.Now;
            membership.PasswordSalt = String.Empty;
            dbContext.aspnet_Membership.Add(membership);
            dbContext.SaveChanges();
            return userNameCheck;
        }

        private string GetPassword()
        {
            throw new NotImplementedException();
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
          
                bool hasPassword = false;
                var aspnetUser = (from u in ShoppingCart.aspnet_User
                                  join
                                  member in ShoppingCart.aspnet_Membership on
                                  u.UserId equals member.UserId
                                  where u.LowerUserName ==
                                  HttpContext.User.Identity.Name.ToLower()

                                  select member)

                                  .FirstOrDefault();

                if (aspnetUser != null)
                    hasPassword = true;
                ViewBag.HasLocalPassword = hasPassword;
                ViewBag.ReturnUrl = Url.Action("Manage");
                return View();
           // }


        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasPassword = false;
            using (ShoppingCart dbContext = new ShoppingCart())
            {

                var aspnetUser = (from u in dbContext.aspnet_User
                                  join
                                  member in dbContext.aspnet_Membership on
                                  u.UserId equals member.UserId
                                  where u.LowerUserName ==
                                  HttpContext.User.Identity.Name.ToLower()

                                  select member)

                                  .FirstOrDefault();

                if (aspnetUser != null)
                    hasPassword = true;
                ViewBag.HasLocalPassword = hasPassword;


                bool hasLocalAccount = hasPassword;
                ViewBag.HasLocalPassword = hasLocalAccount;
                ViewBag.ReturnUrl = Url.Action("Manage");
                if (hasLocalAccount)
                {
                    if (ModelState.IsValid)
                    {
                        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                        bool changePasswordSucceeded;
                        try
                        {

                            CustomMemebership custom = new CustomMemebership(HttpContext.User.Identity.Name);

                            //FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, ConfigurationManager.AppSettings["PasswordFormat"].ToString());
                            changePasswordSucceeded = custom.ChangePassword(model.OldPassword, model.NewPassword);  // WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        }
                        catch (Exception)
                        {
                            changePasswordSucceeded = false;
                        }

                        if (changePasswordSucceeded)
                        {
                            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                        }
                        else
                        {
                            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                        }
                    }
                }
                //else
                //{
                //    // User does not have a local password so remove any validation errors caused by a missing
                //    // OldPassword field
                //    ModelState state = ModelState["OldPassword"];
                //    if (state != null)
                //    {
                //        state.Errors.Clear();
                //    }

                //    if (ModelState.IsValid)
                //    {
                //        try
                //        {
                //            WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                //            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                //        }
                //        catch (Exception e)
                //        {
                //            ModelState.AddModelError("", e);
                //        }
                //    }
                //}

                // If we got this far, something failed, redisplay form
                return View(model);
            }
        }

        // dispose object

        protected override void Dispose(bool disposing)
        {
            if (disposing && ShoppingCart != null)
            {
               // ShoppingCart.Dispose();
               // ShoppingCart = null;
            }

            base.Dispose(disposing);
        }



        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            //  ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            using (ShoppingCart dbContext = new ShoppingCart())
            {
                var externalLogin = dbContext.ExternalLogin.Where(m => m.ProviderDisplayName == HttpContext.User.Identity.Name)
                     .ToList();
                ViewBag.ShowRemoveButton = externalLogin.Count > 1;
                return PartialView("_RemoveExternalLoginsPartial", externalLogin);

            }


            //List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            //foreach (OAuthAccount account in accounts)
            //{
            //    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

            //    externalLogins.Add(new ExternalLogin
            //    {
            //        Provider = account.Provider,
            //        ProviderDisplayName = clientData.DisplayName,
            //        ProviderUserId = account.ProviderUserId,
            //    });
            //}


        }

        [AllowAnonymous]

        [HttpPost]

        public ActionResult ForgetPassword(RegisterModel model)
        {
            ViewData["IsVisible"] = true;

            if (ModelState.IsValidField("Emailid"))
            {
                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    var isEmail = (from u in dbContext.aspnet_User
                                   join
                                   m in dbContext.aspnet_Membership on u.UserId equals m.UserId
                                   where m.Emailid.ToLower() == model.Emailid.ToLower() && u.LowerUserName == model.UserName.ToLower()
                                   select m).FirstOrDefault();
                    if (isEmail == null)
                    {
                        ModelState.AddModelError("", "Your Email ID is not available.");
                    }
                    else
                    {
                        Random random = new Random();
                        string passwordNumber = random.Next().ToString();
                        string password = FormsAuthentication.
                                               HashPasswordForStoringInConfigFile(passwordNumber, ConfigurationManager.
                                               AppSettings["PasswordFormat"].ToString());

                        isEmail.Password = password;

                        dbContext.SaveChanges();
                        ModelState.AddModelError("", "Your Password is " + passwordNumber);

                    }
                }
            }
            return View("Login");
        }

        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            ViewData["IsVisible"] = true;
            return View("Login");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
