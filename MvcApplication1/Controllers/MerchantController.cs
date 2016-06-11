using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.ViewModels;
using MvcApplication1.DataModels;

namespace MvcApplication1.Controllers
{
    public class MerchantController : Controller
    {
        //
        // GET: /Merchant/
        ShoppingCart dbContext = new ShoppingCart();
        public ActionResult Index(MerchantInfo model, string name)
        {
            using (ShoppingCart dbContext = new ShoppingCart())
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        switch (name)
                        {
                            case "Submit":
                                ModelState["Merchant.MerchantCode"].Errors.Clear();
                                if (!ModelState.IsValid) return View(model);
                                long MerchantCode = HomeController.GetEntitySequences((int)EnumsType.EnumTypes.EntityType.Merchant, dbContext);
                                string getSequenceNumber = MerchantCode.ToString().Substring(7);
                                model.User = new aspnet_users();
                                model.User.UserName = getSequenceNumber + "_" + model.Merchant.MerchantName.Replace(" ", "_");
                                model.User.LowerUserName = getSequenceNumber + "_" + model.Merchant.MerchantName.Replace(" ", "_").ToLower();
                                Models.RegisterModel registerModel = new Models.RegisterModel();
                                registerModel.Emailid = model.Address.Emailid;
                                registerModel.UserName = model.User.UserName;
                                registerModel.Password = model.User.UserName;
                                var emailCheck = dbContext.aspnet_Membership.FirstOrDefault(m => m.Emailid.ToLower() == model.Address.Emailid.ToLower());
                                if (emailCheck != null)
                                {
                                    ModelState.AddModelError("", "EmailId already registered");
                                    throw new Exception();
                                }


                                model.User = AccountController.RegisterUser(registerModel, dbContext, model.User);
                                if (model.User.NumericUserId == 0)
                                {
                                    throw new Exception();
                                }
                                model.Merchant.UserId = model.User.NumericUserId;
                                model.Merchant.MerchantCode = MerchantCode;
                                model.Merchant.locationId = model.Address.CityId;
                                model.Merchant.CreatedBy = (dbContext.aspnet_User.FirstOrDefault(m => m.LowerUserName == User.Identity.Name.ToLower()).NumericUserId);
                                model.Merchant.CreateDate = DateTime.Now;

                                MerchantModels merchant = model.Merchant;
                                dbContext.Merchant.Add(merchant);
                                dbContext.SaveChanges();
                                model.Address.AddressTypeId = (int)EnumsType.EnumTypes.AddressType.Permanant;
                                model.Address.EntityId = merchant.MerchantId;
                                model.Address.EntityType = (int)EnumsType.EnumTypes.EntityType.Merchant;
                                dbContext.EntityAddress.Add(model.Address);
                                dbContext.SaveChanges();
                                model = new MerchantInfo();
                                trans.Commit();
                                return RedirectToAction("Index", "Home");

                            default:
                                break;
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        trans.Rollback();
                        throw raise;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return View(model);
                        //throw;
                    }
                }



            }
            return View(model);
        }

        public ActionResult ViewMerchant(int? id)
        {
            if (id != null && id > 0)
            {
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetMerchant(string MerchantName)
        {
            var merchantDetails = (from m in dbContext.Merchant
                                   join e in dbContext.EntityAddress
                                       on m.MerchantId equals e.EntityId 
                                   
                                   where m.MerchantName.Contains(MerchantName) || string.IsNullOrEmpty(MerchantName) || m.MerchantName == MerchantName
                                   && e.EntityType == (int)EnumsType.EnumTypes.EntityType.Merchant
                                   select new { MerchantCode = m.MerchantCode, MerchantName = m.MerchantName, CreatedDate = m.CreateDate,MerchantId = m.MerchantId }

                                       ).ToList();
            return Json(merchantDetails);
        }
    }
}
