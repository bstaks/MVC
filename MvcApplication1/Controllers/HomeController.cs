using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.DataModels;
using MvcApplication1.ViewModels;
using System.Transactions;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(CustomerInfo model, string name)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            using (ShoppingCart dbContext = new ShoppingCart())
            {


                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        switch (name)
                        {
                            case "Submit":
                                ModelState["Customer.CustomerCode"].Errors.Clear();
                                if (!ModelState.IsValid) return View(model);
                                long CustomerCode = GetEntitySequences((int)EnumsType.EnumTypes.EntityType.Customer, dbContext);
                                string getSequenceNumber = CustomerCode.ToString().Substring(7);
                                model.User = new aspnet_users();
                                model.User.UserName = getSequenceNumber + "_" + model.Customer.CustomerName.Replace(" ", "_");
                                model.User.LowerUserName = getSequenceNumber + "_" + model.Customer.CustomerName.Replace(" ", "_").ToLower();
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
                                model.Customer.UserId = model.User.NumericUserId;
                                model.Customer.CustomerCode = CustomerCode;
                                model.Customer.locationId = model.Address.CityId;
                                model.Customer.CreatedBy = (dbContext.aspnet_User.FirstOrDefault(m => m.LowerUserName == User.Identity.Name.ToLower()).NumericUserId);
                                model.Customer.CreateDate = DateTime.Now;

                                CustomerModels customer = model.Customer;
                                dbContext.Customer.Add(customer);
                                dbContext.SaveChanges();
                                model.Address.AddressTypeId = (int)EnumsType.EnumTypes.AddressType.Permanant;
                                model.Address.EntityId = customer.CustomerId;
                                model.Address.EntityType = (int)EnumsType.EnumTypes.EntityType.Customer;
                                EntityAddressModels entityAddress = model.Address;
                                dbContext.EntityAddress.Add(entityAddress);
                                dbContext.SaveChanges();
                                model = new CustomerInfo();
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

        public static long GetEntitySequences(int entityType, ShoppingCart dbContext)
        {
            long entitySequence = 0;
            SqlParameter[] param = {
        new SqlParameter("@EntityType",entityType),
        };
            var entitySequences = dbContext.Database.SqlQuery<EntitySequence>("dbo.uspGetEntitySequences @EntityType", param).FirstOrDefault();

            if (entitySequences != null && ((DateTime.Now.Date -
                entitySequences.CreateDate.Date).Days) == 0)
            {
                entitySequences = dbContext.EntitySequence.FirstOrDefault(m => m.SequenceNumber == entitySequences.SequenceNumber);
                entitySequences.SequenceNumber = entitySequences.SequenceNumber + 1;
                dbContext.SaveChanges();
                return entitySequences.SequenceNumber;
            }

            switch (entityType)
            {
                case (int)EnumsType.EnumTypes.EntityType.Customer:
                    entitySequence = 1000000000;

                    break;
                case (int)EnumsType.EnumTypes.EntityType.Merchant:
                    entitySequence = 2000000000;

                    break;

                default:
                    break;
            }

            if (entitySequences != null && (DateTime.Now.Date - entitySequences.CreateDate.Date).Days >= 1 && entitySequences.EntityType == entityType)
            {

                entitySequence = entitySequences.SequenceNumber + 1;
            }

            entitySequences = new EntitySequence();
            entitySequences.SequenceNumber = entitySequence;
            entitySequences.CreateDate = DateTime.Now;
            entitySequences.EntityType = entityType;

            dbContext.EntitySequence.Add(entitySequences);
            dbContext.SaveChanges();
            return entitySequences.SequenceNumber;


        }




        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        

        public ActionResult ViewCustomer( int id = 0,string customerName=null,int pageSize = 10, int pageNumber=0)
        {
            if (id > 0)
            {
                return View("CustomerDetails");
            }
            return View(); 
        }

       

        
        public ActionResult GetCustomer(string customerName)
        {
            int pageSize = 10;
            int pageNumber = 0;
            using (ShoppingCart dbContext = new ShoppingCart())
            {

                var customer = dbContext.Customer.Where(m => m.CustomerName.Contains(customerName) ||
                    string.IsNullOrEmpty(customerName) || m.CustomerName == customerName)
                    .Select(m => new {CustomerName = m.CustomerName,CustomerId = m.CustomerId,CustomerCode = m.CustomerCode, CreatedDate = m.CreateDate }).ToList()//.Skip(pageNumber * pageSize).Take(pageSize)
                    .ToList();
                return Json(customer,JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ViewCustomer_Backup(FormCollection col, int pageNumber=0)
        {
            int pages = 10;
            Regex reg = new Regex("^(1)[0-9]{9}$");
            using (ShoppingCart dbContext = new ShoppingCart())
            {
                string CustomerCode = string.Empty;
                CustomerCode = col["txtCustomerCode"];
                ViewBag.CustomerCode = CustomerCode;
                
                //if (string.IsNullOrEmpty(CustomerCode))
                //{
                //    ModelState.AddModelError("txtCustomerCode", "Customer Code is required!");
                //    return View();
                //}
                //if (!reg.IsMatch(CustomerCode))
                //{
                //    ModelState.AddModelError("txtCustomerCode", "Customer Code is invalid!");
                //    return View();
                //}
                List<CustomerModels> CustomerList = dbContext.Customer.Select(m => m).ToList();
                ViewBag.TotalRecord = 400;//CustomerList.Count;
                CustomerList = CustomerList.Skip((pageNumber * pages) + 1).Take(pages).ToList();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_PartialCustomerView",CustomerList);
                }
                
                return View(CustomerList);   
            }
            
        }


        public ActionResult Test()
        {
            TempData["Hi"] = "Hi";
            return RedirectToAction("Test2");
        }

        public ActionResult Test2()
        {
           
            string tempData = Convert.ToString(TempData["Hi"]);
            TempData.Peek("Hi");
            return View("Test");
        }
    }

   
}


