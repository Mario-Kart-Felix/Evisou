using Evious.Account.Contract;
using Evious.Crm.Contract;
using Evious.Framework.Contract;
using Evious.Framework.Utility;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Crm.Controllers
{
    [Permission(EnumBusinessPermission.CrmManage_Project)]
    public class ProjectController : AdminControllerBase
    {

        public ActionResult Datatable(ProjectRequest request)
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
                                this.CrmService.DeleteProject(ids);
                                break;
                        }
                        break;

                    case "delete":
                        this.CrmService.DeleteProject(ids);
                        break;
                }
            }

            #endregion

            var allProject = this.CrmService.GetProjectList(null);

            IEnumerable<Project> filterProject = allProject;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).ToLower().Trim();

                var CreateTimeFilter =Request["createtime"];                

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterProject = filterProject.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
                var isNnmberSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());

                if (isNnmberSearchable && !string.IsNullOrEmpty(CreateTimeFilter))
                {
                    CreateTimeFilter = DateTime.Parse(Convert.ToString(Request["createtime"]).ToLower().Trim()).ToCnDataString();
                    filterProject = filterProject.Where(c => c.CreateTime.ToCnDataString().ToLower().Contains(CreateTimeFilter));
                }
            }
            else if (request.action == "filter_cancel")
            {
                filterProject = allProject;
            }
            else
            {
                filterProject = allProject;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isCreatetimeSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Project, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ?c.Name :
                                                sortColumnIndex == 2 && isCreatetimeSortable ? c.CreateTime.ToCnDataString() :                                                
                                                "");

            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterProject = filterProject.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterProject = filterProject.OrderByDescending(orderingFunction);
            }

            #endregion


            var displayedProject = filterProject.Skip(request.start).Take(request.length);
            var result = from c in displayedProject
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.CreateTime.ToCnDataString()                                       
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allProject.Count(),//alltransactions.Count(),
                recordsFiltered = filterProject.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Crm/Project/

        public ActionResult Index(ProjectRequest request)
        {
            var result = this.CrmService.GetProjectList(request);
            return View(result);
        }

        //
        // GET: /Crm/Project/Create

        public ActionResult Create()
        {
            var model = new Project() { };
            return View("Edit", model);
        }

        //
        // POST: /Crm/Project/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Project();
            this.TryUpdateModel<Project>(model);
            try
            {
                if (ModelState.IsValid)
                {
                    this.CrmService.SaveProject(model);
                }
                string url = Url.Action("Index", "Project");
                return Json(new { success = true, url = url });
            }

            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                
                return View("Edit", model);
            }
        }

        //
        // GET: /Crm/Project/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.CrmService.GetProject(id);
            return View(model);
        }

        //
        // POST: /Crm/Project/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.CrmService.GetProject(id);
            this.TryUpdateModel<Project>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.CrmService.SaveProject(model);
                }
                string url = Url.Action("Index", "Project");
                return Json(new { success = true, url = url });
            }

            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);

                return View("Edit", model);
            }
        }

        // POST: /Crm/Project/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.CrmService.DeleteProject(ids);
            return RedirectToAction("Index");
        }
    }
}