using Evious.Account.Contract;
using Evious.Framework.Contract;
using Evious.OA.Contract;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.OA.Controllers
{
    [Permission(EnumBusinessPermission.OAManage_Branch)]
    public class BranchController : AdminControllerBase
    {
        public JsonResult Datatable(BranchRequest request)
        {


            #region 自定义动作
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
                                this.OAService.DeleteBranch(ids);
                                break;                           
                        }
                        break;

                    case "delete":
                        this.OAService.DeleteBranch(ids);
                        break;
                }
            }

            #endregion

            var allBranch = this.OAService.GetBranchList(null);

            IEnumerable<Branch> filterBranch = allBranch;


            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).Trim();

                var DescFilter = Convert.ToString(Request["desc"]).Trim();

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterBranch = filterBranch.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
                var isDescSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());

                if (isDescSearchable)
                {
                    filterBranch = filterBranch.Where(c => c.Desc.ToLower().Contains(DescFilter));
                }

                

            }
            else if (request.action == "filter_cancel")
            {
                filterBranch = allBranch;
            }
            else
            {
                filterBranch = allBranch;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isDescSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isActiveSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Branch, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                sortColumnIndex == 2 && isDescSortable ? c.Desc :                                               
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterBranch = filterBranch.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterBranch = filterBranch.OrderByDescending(orderingFunction);
            }

            #endregion


            var displayedBranch = filterBranch.Skip(request.start).Take(request.length);
            var result = from c in displayedBranch
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.Desc   
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allBranch.Count(),//alltransactions.Count(),
                recordsFiltered = filterBranch.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /OA/Branch/

        public ActionResult Index(BranchRequest request)
        {
            var result = this.OAService.GetBranchList(request);
            return View(result);
        }

        //
        // GET: /OA/Branch/Create

        public ActionResult Create()
        {
            var model = new Branch();
            return View("Edit", model);
        }

        //
        // POST: /OA/Branch/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Branch();
            this.TryUpdateModel<Branch>(model);
            
            try
            {
                if (ModelState.IsValid)
                {
                    this.OAService.SaveBranch(model);
                    string url = Url.Action("Index", "Branch");
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
        // GET: /OA/Branch/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.OAService.GetBranch(id);
            return View(model);
        }

        //
        // POST: /OA/Branch/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.OAService.GetBranch(id);
            this.TryUpdateModel<Branch>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.OAService.SaveBranch(model);
                    string url = Url.Action("Index", "Branch");
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

        // POST: /OA/Branch/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.OAService.DeleteBranch(ids);
            return RedirectToAction("Index");
        }
    }
}