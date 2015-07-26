using Evisou.Account.Contract;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using Evisou.Framework.Web;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Account.Controllers
{
    [Permission(EnumBusinessPermission.AccountManage_User)]
    public class UserController : AdminControllerBase
    {

        public JsonResult Datatable(UserRequest request)
        {

            if (!string.IsNullOrEmpty(request.customActionType))
            {
                string[] id_Array = Request.Params.GetValues("id[]");
                List<int> ids = new List<int>();
                foreach (string i in id_Array)
                {
                    ids.Add(int.Parse(i));
                }
                
                switch (request.customActionType)
                {
                    case "group_action":
                        
                        switch (request.customActionName)
                        {
                            case "delete":
                                this.AccountService.DeleteUser(ids);
                                break;
                            case "freeze":
                                foreach (var id in ids)
                                {
                                    var model = this.AccountService.GetUser(id);
                                    model.IsActive = false;
                                    this.TryUpdateModel<User>(model);
                                    this.AccountService.SaveUser(model);
                                }
                                break;

                            case "active":
                                foreach (var id in ids)
                                {
                                    var model = this.AccountService.GetUser(id);
                                    model.IsActive = true;
                                    this.TryUpdateModel<User>(model);
                                    this.AccountService.SaveUser(model);
                                }
                                break;
                        }
                        break;

                    case "delete":                       
                        this.AccountService.DeleteUser(ids);
                        break;
                }
            }

            

            var allUsers = this.AccountService.GetUserList(null);

            IEnumerable<User> filterUsers = allUsers;
            if (!string.IsNullOrEmpty(request.search))
            {
                var isLoginNameSearchable = Convert.ToBoolean(Request["columns[0][searchable]"]);
                var isEmailSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);
                var isMoblieNameSearchable = Convert.ToBoolean(Request["columns[2][searchable]"]);
                var isRoleNameSearchable = Convert.ToBoolean(Request["columns[3][searchable]"]);
                var isActiveNameSearchable = Convert.ToBoolean(Request["columns[4][searchable]"]);
            }
            else if (request.action == "filter")
            {
                var loginNameFilter = Convert.ToString(Request["loginname"]);
                var EmailFilter = Convert.ToString(Request["email"]);
                var MoblieFilter = Convert.ToString(Request["phone"]);
                var RoleFilter = Convert.ToString(Request["roles"]);
                var ActiveFilter = Convert.ToString(Request["isactive"]);

                var isLoginNameSearchable = string.IsNullOrEmpty(Request["columns[1][searchable]"].ToString()) ? false : true;
                var isEmailSearchable = string.IsNullOrEmpty(Request["columns[2][searchable]"].ToString()) ? false : true;
                var isMoblieNameSearchable = string.IsNullOrEmpty(Request["columns[3][searchable]"].ToString()) ? false : true;
                var isRoleNameSearchable = string.IsNullOrEmpty(Request["columns[4][searchable]"].ToString()) ? false : true;
                var isActiveNameSearchable = false;              
                if (!string.IsNullOrEmpty(Request["isactive"].ToString()))
                {
                    isActiveNameSearchable = string.IsNullOrEmpty(Request["columns[5][searchable]"].ToString()) ? false : true;
                }

                if (isLoginNameSearchable)
                {
                    filterUsers = filterUsers.Where(c => c.LoginName.ToLower().Contains(loginNameFilter));
                }

                if (isEmailSearchable)
                {
                    filterUsers = filterUsers.Where(c => c.Email.ToLower().Contains(EmailFilter));
                }

                if (isMoblieNameSearchable)
                {
                    filterUsers = filterUsers.Where(c => c.Mobile.ToLower().Contains(MoblieFilter));
                }

                
                if (isActiveNameSearchable)
                {
                    filterUsers = filterUsers.Where(c => c.IsActive == ActiveFilter.ToBool());
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterUsers = allUsers;
            }
            else
            {
                filterUsers = allUsers;
            }

            var isLoginNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isEmailSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isMoblieSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
            var isRoleSortable = Convert.ToBoolean(Request["columns[4][orderable]"]);
            var isActiveSortable = Convert.ToBoolean(Request["columns[5][orderable]"]);

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<User, string> orderingFunction = (c =>
                                                           sortColumnIndex == 1 && isLoginNameSortable ? c.LoginName :
                                                           sortColumnIndex == 2 && isEmailSortable ? c.Email :
                                                           sortColumnIndex == 3 && isMoblieSortable ? c.Mobile :
                                                               // sortColumnIndex == 4 && isRoleSortable ? c. :
                                                           sortColumnIndex == 5 && isActiveSortable ? c.IsActive.ToString() :
                                                           "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterUsers = filterUsers.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterUsers = filterUsers.OrderByDescending(orderingFunction);
            }

            var displayedUser = filterUsers.Skip(request.start).Take(request.length);//.Where(a => a.ID != AdminUserContext.Current.LoginInfo.ID);
            var result = from c in displayedUser
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.LoginName
                                            , c.Email
                                            , c.Mobile
                                            , StringUtil.CutString(string.Join(",", c.Roles.Select(r => r.Name)), 40)
                                            , c.IsActive.ToString()
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allUsers.Count(),//alltransactions.Count(),
                recordsFiltered = filterUsers.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Account/User/

        public ActionResult Index(UserRequest request)
        {
            var result = this.AccountService.GetUserList(request);
           
            return View(result);
        }

    
        //
        // GET: /Account/User/Create

        public ActionResult Create()
        {
            var roles = this.AccountService.GetRoleList();
            this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name");            
            var model = new User();
            model.Password = "111111";
            return PartialView("Edit", model);
        }

        //
        // POST: /Account/User/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new User();
            this.TryUpdateModel<User>(model);
            model.Password = "111111";
            model.Password = Encrypt.MD5(model.Password);
            var roles = this.AccountService.GetRoleList();
           
            try
            {
                if (ModelState.IsValid)
                {
                    this.AccountService.SaveUser(model);
                    //return RedirectToAction("Index"); 
                    string url = Url.Action("Index", "User");
                    return Json(new { success = true, url = url });
                }
                 this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name");
                return PartialView("Edit", model);
                
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);                
                this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name");
                //return null;
                return PartialView("Edit", model);
            }

            
        }

        //
        // GET: /Account/User/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.AccountService.GetUser(id);            
            var roles = this.AccountService.GetRoleList();
            this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name", string.Join(",", model.Roles.Select(r => r.ID)));

            return PartialView(model);
        }

        //
        // POST: /Account/User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.AccountService.GetUser(id);
            this.TryUpdateModel<User>(model);
            this.AccountService.SaveUser(model);
            var roles = this.AccountService.GetRoleList();

            try
            {
                if (ModelState.IsValid)
                {
                    this.AccountService.SaveUser(model);
                    //return RedirectToAction("Index"); 
                    string url = Url.Action("Index", "User");
                    return Json(new { success = true, url = url });
                }
                this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name");
                return PartialView("Edit", model);

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name");
                //return null;
                return PartialView("Edit", model);
            }
        }

        // POST: /Account/User/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            
            this.AccountService.DeleteUser(ids);
            return RedirectToAction("Index");
        }

        
        public ActionResult MyProfile()
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            var roles = this.AccountService.GetRoleList();
            this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name", string.Join(",", model.Roles.Select(r => r.ID)));
            return PartialView(model);
        }

        [HttpPost]        
        public ActionResult MyProfile(FormCollection collection)
        {
            int id = this.LoginInfo.UserID;
            User model = this.AccountService.GetUser(id);
            this.TryUpdateModel<User>(model);
            var roles = this.AccountService.GetRoleList();
            try
            {
                if (ModelState.IsValid)
                {
                    this.AccountService.SaveUser(model);  
                }
                this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name", string.Join(",", model.Roles.Select(r => r.ID)));
                return PartialView("MyProfile", model);

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name", string.Join(",", model.Roles.Select(r => r.ID)));
                return PartialView("MyProfile", model);
            }

        }
    
      
    }
}