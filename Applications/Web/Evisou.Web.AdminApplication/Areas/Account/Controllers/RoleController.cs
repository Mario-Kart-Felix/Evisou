using Evisou.Account.Contract;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Account.Controllers
{
    [Permission(EnumBusinessPermission.AccountManage_Role)]
    public class RoleController : AdminControllerBase
    {


        public JsonResult Datatable(RoleRequest request)
        {
            #region 自定义动作
            if (!string.IsNullOrEmpty(request.customActionType))
            {
                List<int> ids = DatatableHelper.ArrayStringToListInt(Request.Params.GetValues("id[]"));

                switch (request.customActionType)
                {
                    case "group_action":

                        switch (request.customActionName)
                        {
                            case "delete":
                                this.AccountService.DeleteRole(ids);
                                break;
                        }
                        break;

                    case "delete":
                        this.AccountService.DeleteRole(ids);
                        break;
                }
            }
            #endregion

            var allRoles = this.AccountService.GetRoleList(null);

            IEnumerable<Role> filterRoles = allRoles;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[0][searchable]"]);
                
            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]);
               

                var isNameSearchable = string.IsNullOrEmpty(Request["columns[1][searchable]"].ToString()) ? false : true;
                
                

                if (isNameSearchable)
                {
                    filterRoles = filterRoles.Where(c => c.Name.ToLower().Contains(NameFilter));
                }

               

            }
            else if (request.action == "filter_cancel")
            {
                filterRoles = allRoles;
            }
            else
            {
                filterRoles = allRoles;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            
            

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Role, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterRoles = filterRoles.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterRoles = filterRoles.OrderByDescending(orderingFunction);
            }
            #endregion

            var displayedRoles = filterRoles.Skip(request.start).Take(request.length);
            var result = from c in displayedRoles
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.Info                                            
                                            , StringUtil.CutString(string.Join(",", c.BusinessPermissionList.Select(r => Evisou.Framework.Utility.EnumHelper.GetEnumTitle(r))), 40)                                           
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allRoles.Count(),//alltransactions.Count(),
                recordsFiltered = filterRoles.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
               

        //
        // GET: /Account/Role/

        public ActionResult Index(RoleRequest request)
        {
            var result = this.AccountService.GetRoleList(request);
            return View(result);
        }

        //
        // GET: /Account/Role/Create

        public ActionResult Create()
        {
            var businessPermissionList = EnumHelper.GetItemValueList<EnumBusinessPermission>();
            this.ViewBag.BusinessPermissionList = new SelectList(businessPermissionList, "Key", "Value");

            var model = new Role();
            return View("Edit", model);
        }

        //
        // POST: /Account/Role/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
           
            var model = new Role();
            this.TryUpdateModel<Role>(model);
            try
            {
                if (ModelState.IsValid)
                {
                    this.AccountService.SaveRole(model);
                    string url = Url.Action("Index", "Auth");
                    return Json(new { success = true, url = url });
                }
                return PartialView("Edit", model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return PartialView("Edit", model);
            }            
        }

        //
        // GET: /Account/Role/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.AccountService.GetRole(id);

            var businessPermissionList = EnumHelper.GetItemValueList<EnumBusinessPermission>();
            this.ViewBag.BusinessPermissionList = new SelectList(businessPermissionList, "Key", "Value", string.Join(",", model.BusinessPermissionList.Select(p => (int)p)));

            return View(model);
        }

        //
        // POST: /Account/Role/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.AccountService.GetRole(id);
            this.TryUpdateModel<Role>(model);
            model.BusinessPermissionString = collection["BusinessPermissionList"];
           
            try
            {
                if (ModelState.IsValid)
                { 
                 this.AccountService.SaveRole(model);
                 string url = Url.Action("Index", "Auth");
                 return Json(new { success = true, url = url });
                }
                return PartialView("Edit", model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);    
                return PartialView("Edit", model);
            }
            //return this.RefreshParent();
        }

        // POST: /Account/Role/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.AccountService.DeleteRole(ids);
            return RedirectToAction("Index");
        }
    }
}