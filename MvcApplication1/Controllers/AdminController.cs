﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.DataModels;
using System.Data.SqlClient;
using System.Dynamic;
using MvcApplication1.ViewModels;

namespace MvcApplication1.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        [HttpPost]
        public ActionResult AddValueTypeGroup(ValueTypeGroupModels model)
        {
            if (ModelState.IsValid)
            {
                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    model.CaretedDate = DateTime.Now;
                    model.CreatedBy = 1;
                    model.Active = true;
                    dbContext.ValueTypeGroup.Add(model);
                    dbContext.SaveChanges();
                    return View(new ValueTypeGroupModels());

                }

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddValueType(ValueTypeModels model)
        {
            if (ModelState.IsValid)
            {
                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    model.Active = true;
                    model.SortOrder = dbContext.ValueType.Where(m => m.ValueTypeGroupId == model.ValueTypeGroupId).Select(m => m).ToList().Count > 0 ?
                      (dbContext.ValueType.Where(m => m.ValueTypeGroupId == model.ValueTypeGroupId).ToList().Max(m => m.SortOrder)) + 1 : 1;
                    SqlParameter[] parameter = { 
                                               
                                               new SqlParameter("@ValueTypeId",model.ValueTypeId),
                    new SqlParameter("@ValueTypeName",model.ValueTypeName),
                    new SqlParameter("@ValueTypeGroupId",model.ValueTypeGroupId),
                    new SqlParameter("@SortOrder",model.SortOrder),
                    new SqlParameter("@Active",model.Active)
                                               };

                    dbContext.Database
                        .ExecuteSqlCommand
                        ("Insert into ValueType (ValueTypeId,ValueTypeName,SortOrder,Active,ValueTypeGroupId) values(@ValueTypeId,@ValueTypeName,@SortOrder,@Active,@ValueTypeGroupId)", parameter);
                    //   dbContext.SaveChanges();
                    ModelState.Clear();
                    return View(new ValueTypeModels());

                }

            }

            return View(model);
        }
        [HttpGet]
        public ActionResult AddValueType()
        {
            return View();
        }

        public ActionResult AddMenu()
        {
            return View();
        }

        public ActionResult ViewValueType()
        {

            dynamic d = new ExpandoObject();

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                d.ValueType = (from vg in dbContext.ValueTypeGroup
                               join
                               v in dbContext.ValueType
                               on vg.ValueTypeGroupId equals v.ValueTypeGroupId
                               select new { v }).ToList();
                d.ValueTypeGroup = (from vg in dbContext.ValueTypeGroup
                                    join
                                    v in dbContext.ValueType
                                    on vg.ValueTypeGroupId equals v.ValueTypeGroupId
                                    select new { vg }).ToList();
                //    var t =   model.vg;
                return View(d);
            }

        }

        [ChildActionOnly]
        [OutputCache(Duration = 30000)]
        public ActionResult _PartialMenu()
        {

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                var menu = dbContext.Menu.Select(m => m).ToList();
                return PartialView(menu);
            }


        }

        [HttpGet]
        public ActionResult AddRole(string Id)
        {

            if (!string.IsNullOrEmpty(Id))
            {
                if (Request.QueryString["RoleName"] != null)
                {
                    ViewBag.RoleName = Request.QueryString["RoleName"].ToString();
                }

                return View(MenuPermission(Id));
            }
            return AddRole(null, null, null);
        }


        public bool isPermission(int permission, string ActionType)
        {
            if ((permission & (int)EnumsType.EnumTypes.PermissionType.View) == (int)EnumsType.EnumTypes.PermissionType.View && ActionType == Enum.GetName(typeof(MvcApplication1.EnumsType.EnumTypes.PermissionType), (int)EnumsType.EnumTypes.PermissionType.View))
            {
                return true;
            }
            if ((permission & (int)EnumsType.EnumTypes.PermissionType.Add) == (int)EnumsType.EnumTypes.PermissionType.Add && ActionType == Enum.GetName(typeof(MvcApplication1.EnumsType.EnumTypes.PermissionType), (int)EnumsType.EnumTypes.PermissionType.Add))
            {

                return true;
            }
            if ((permission & (int)EnumsType.EnumTypes.PermissionType.Edit) == (int)EnumsType.EnumTypes.PermissionType.Edit && ActionType == Enum.GetName(typeof(MvcApplication1.EnumsType.EnumTypes.PermissionType), (int)EnumsType.EnumTypes.PermissionType.Edit))
            {
                return true;
            }

            if ((permission & (int)EnumsType.EnumTypes.PermissionType.Delete) == (int)EnumsType.EnumTypes.PermissionType.Delete && ActionType == Enum.GetName(typeof(MvcApplication1.EnumsType.EnumTypes.PermissionType), (int)EnumsType.EnumTypes.PermissionType.Delete))
            {
                return true;
            }

            return false;
        }



        [HttpPost]
        public ActionResult AddRole(FormCollection col, List<MvcApplication1.CustomModel.MenuPermissionInfo> model, string btnSubmit)
        {
            List<MvcApplication1.CustomModel.MenuPermissionInfo> objMenuPermission = new List<CustomModel.MenuPermissionInfo>();

            SqlParameter[] param = { };
            if (!string.IsNullOrEmpty(btnSubmit))
            {
                ViewBag.RoleName = col["txtRoleName"].ToString();

                string[] str = { };
                string[] checkBoxValue = { };
                foreach (var KeyValue in col.AllKeys)
                {
                    var value = col[KeyValue];
                    str = value.Split(',');
                    if (str.Length > 1)
                    {
                        checkBoxValue = str[0].Split('|');
                        MvcApplication1.CustomModel.MenuPermissionInfo menupermissionList = new CustomModel.MenuPermissionInfo();
                        menupermissionList.MenuId = Convert.ToInt16(checkBoxValue[1]);
                        if (objMenuPermission.FirstOrDefault(m => m.MenuId == menupermissionList.MenuId) != null)
                        {
                            var menu = objMenuPermission.FirstOrDefault(m => m.MenuId == menupermissionList.MenuId);
                            objMenuPermission.Remove(menu);
                            menupermissionList.Permission = menu.Permission + Convert.ToInt16(checkBoxValue[0]);
                            objMenuPermission.Add(menupermissionList);
                            continue;
                        }
                        menupermissionList.Permission = Convert.ToInt16(checkBoxValue[0]);
                        objMenuPermission.Add(menupermissionList);
                    }

                }

                // update Role




                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    var menuList = dbContext.Database.SqlQuery<MvcApplication1.CustomModel.MenuPermissionInfo>("uspGetMenu", param).ToList();

                    if (!string.IsNullOrEmpty(ViewBag.RoleName))
                    {
                        string roleName = ViewBag.RoleName;
                        aspnet_Role role = dbContext.aspnet_Role.FirstOrDefault(m => m.RoleName.ToLower() == roleName.ToLower());
                        if (role != null)
                        {
                            // update role 

                            role.RoleName = ViewBag.RoleName;
                            dbContext.SaveChanges();

                            List<aspnet_MenuPermission> menuPermission = dbContext.aspnet_MenuPermission.Where(m => m.RoleId == role.RoleId).ToList();
                            foreach (aspnet_MenuPermission permission in menuPermission)
                            {
                                dbContext.aspnet_MenuPermission.Remove(permission);
                                dbContext.SaveChanges();
                            }
                            AddPermissionForMenu(objMenuPermission, dbContext, role, 1);
                            // end update role
                            // ModelState.AddModelError("", "Role Already exists!");
                            return AddRole(role.RoleId.ToString());
                        }
                        role = new aspnet_Role();
                        role.RoleId = Guid.NewGuid();
                        role.RoleName = ViewBag.RoleName;
                        dbContext.aspnet_Role.Add(role);
                        dbContext.SaveChanges();
                        int createdBy = (dbContext.aspnet_User.FirstOrDefault(m => m.UserName.ToLower() == HttpContext.User.Identity.Name.ToLower()).NumericUserId);
                        AddPermissionForMenu(objMenuPermission, dbContext, role, createdBy);
                        return AddRole(role.RoleId.ToString());
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("", "Role Name is required!");
                    }
                    return View(menuList);
                }


            }
            else
            {
                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    var menuList = dbContext.Database.SqlQuery<MvcApplication1.CustomModel.MenuPermissionInfo>("uspGetMenu", param).ToList();
                    return View(menuList);
                }
            }
            //SqlHelpers.SqlHelper.ExecuteReader(System.Data.CommandType.StoredProcedure,"")

        }

        private static void AddPermissionForMenu(List<MvcApplication1.CustomModel.MenuPermissionInfo> objMenuPermission, ShoppingCart dbContext, aspnet_Role role, int createdBy)
        {
            foreach (var menuPermission in objMenuPermission)
            {
                aspnet_MenuPermission desmenuPermission = new aspnet_MenuPermission();
                desmenuPermission.Active = true;
                desmenuPermission.CreatedDate = DateTime.Now;
                desmenuPermission.MenuId = menuPermission.MenuId;
                desmenuPermission.Permission = menuPermission.Permission;
                desmenuPermission.RoleId = role.RoleId;
                desmenuPermission.CreatedBy = createdBy;
                dbContext.aspnet_MenuPermission.Add(desmenuPermission);
                dbContext.SaveChanges();
            }
        }




        public ActionResult ViewRole(string roleName, int pageNumber = 0)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    return Json(dbContext.aspnet_Role.Select(m => m).ToList());
                }
            }

            return View();
        }


        public ActionResult DetailsRole(string Id)
        {
            MenuPermission(Id);

            return View();


        }


        private List<MvcApplication1.CustomModel.MenuPermissionInfo> MenuPermission(string Id)
        {
            SqlParameter[] parameter = { new SqlParameter("@RoleId", Id) };

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                List<MvcApplication1.CustomModel.MenuPermissionInfo> menuPermission = dbContext.Database.SqlQuery<MvcApplication1.CustomModel.MenuPermissionInfo>("uspGetMenuPermissionbyRoleId @RoleId", parameter).ToList();
                ViewBag.MenuPermission = Newtonsoft.Json.JsonConvert.SerializeObject(menuPermission, Newtonsoft.Json.Formatting.Indented);
                return menuPermission;
                //return View(menuPermission);
            }
        }

        public ActionResult EditRole(string Id)
        {
            MenuPermission(Id);
            return View();
        }

        public ActionResult AddRoleToUser(Guid? userId = null)
        {
            // Guid uGuid = userId.HasValue ? Guid.NewGuid() : userId.Value;// new Guid("C56D127A-2C33-42FA-AD09-2B5EE715BE17");
            //using (ShoppingCart dbContext = new ShoppingCart())
            //{
            //    var roles = dbContext.aspnet_Role.Select(m => new { m.RoleId, m.RoleName }).ToList();
            //    ViewBag.Roles = Newtonsoft.Json.JsonConvert.SerializeObject(roles);
            //}
            //Guid uGuid = Guid.NewGuid();
            //if (TempData["userId"] != null)
            //{
            //    uGuid = (Guid)TempData["userId"];
            //    TempData.Keep("userId");
            //}

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                var roles = dbContext.aspnet_Role.Select(m => new
                {
                    m.RoleId,
                    m.RoleName,
                    isChecked =
                        ((dbContext.aspnet_UserInRoles.FirstOrDefault(u => userId != null && u.UserId == userId && m.RoleId == u.RoleId).UserId == userId && userId != null) ? "selected" : "")
                }).ToList();
                ViewBag.Roles = Newtonsoft.Json.JsonConvert.SerializeObject(roles);
            }

            return View();
        }



        // Send Id through Temp data then compare userId
        [ActionName("AddRoleToUser")]
        [HttpPost]
        public ActionResult AddRoleToUserPost()
        {
            Guid uGuid = new Guid("C56D127A-2C33-42FA-AD09-2B5EE715BE17");
            //if (TempData["userId"] != null)
            //{
            //    uGuid = (Guid)TempData["userId"];
            //    TempData.Keep("userId");
            //}

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                var roles = dbContext.aspnet_Role.Select(m => new
                {
                    m.RoleId,
                    m.RoleName,
                    isChecked =
                        (dbContext.aspnet_UserInRoles.FirstOrDefault(u => u.UserId == uGuid && m.RoleId == u.RoleId).UserId == uGuid)
                }).ToList();
                ViewBag.Roles = Newtonsoft.Json.JsonConvert.SerializeObject(roles);
            }

            return View();
        }

        [HttpPost]
        public JsonResult AddRoleToUser(string[] rolesId, string userId)
        {
            int result = 0;

            Guid userGuid = new Guid(userId);

            using (ShoppingCart dbContext = new ShoppingCart())
            {
                List<aspnet_UsersInRoles> objExitsUserInRoles = new List<aspnet_UsersInRoles>();
                objExitsUserInRoles = dbContext.aspnet_UserInRoles.Where(m => m.UserId == userGuid).ToList();
                dbContext.aspnet_UserInRoles.RemoveRange(objExitsUserInRoles);
                dbContext.SaveChanges();
                foreach (string roleId in rolesId)
                {
                    aspnet_UsersInRoles userInRole = new aspnet_UsersInRoles();
                    userInRole.RoleId = new Guid(roleId);
                    userInRole.UserId = userGuid;
                    dbContext.aspnet_UserInRoles.Add(userInRole);
                    result = dbContext.SaveChanges();
                }
            }


            return Json(result);
        }

        [Authorize]
        [HttpGet]
        public JsonResult GetAllUsers(string q)
        {
            using (ShoppingCart dbContext = new ShoppingCart())
            {
                return Json(dbContext.aspnet_User.Where(m => m.UserName.Contains(q)).Select(m => new { m.UserName, m.UserId }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ViewUser(string UserName = null, string commanName = null)
        {
            if (commanName == "SearchUsers")
            {
                using (ShoppingCart dbContext = new ShoppingCart())
                {
                    //var result = dbContext.aspnet_User.Include("UserInRoles").Select(m => new { UserName = m.UserName, UserInRoles = string.Join(",", (from r in dbContext.aspnet_Role join uin in m.UserInRoles on r.RoleId equals uin.RoleId select r).ToList()) });
                    //var result = dbContext.aspnet_User.Include("UserInRoles").Select(m => m).ToList();
                    //var userInRoles = dbContext.aspnet_User.Include("UserInRoles").Select(m => new MvcApplication1 .Models.ViewUserModels() { UserName = m.UserName, RoleNames  = string.Join(",", (from r in dbContext.aspnet_Role join uin in m.UserInRoles on r.RoleId equals uin.RoleId select r).ToList()) });             
                    //return Json(dbContext.aspnet_User.Include("UserInRoles").Select(m => new { UserName = m.UserName, UserInRoles = string.Join(",", (from r in dbContext.aspnet_Role join uin in m.UserInRoles on r.RoleId equals uin.RoleId select r).ToList()) }), JsonRequestBehavior.AllowGet);

                    SqlParameter[] parameter = { };
                    var result = dbContext.Database.SqlQuery<MvcApplication1.Models.ViewUserModels>("uspGetUserWithRole", parameter).ToList();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            return View();
        }


    }
}
