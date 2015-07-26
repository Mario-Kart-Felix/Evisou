using Evisou.Account.Contract;
using Evisou.Framework.Contract;
using Evisou.Ims.Contract.Model;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Agent)]
    public class AgentController : AdminControllerBase
    {
        public JsonResult Datatable(SupplierRequest request)
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
                                this.ImsService.DeleteAgent(ids);
                                break;

                        }
                        break;

                    case "delete":
                        this.ImsService.DeleteAgent(ids);
                        break;
                }
            }
            #endregion

            var allAgents = this.ImsService.GetAgentList(null);

            IEnumerable<Agent> filterAgents = allAgents;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).ToLower().Trim();
                var CodeFilter = Convert.ToString(Request["code"]).ToLower().Trim();
                var ConsigneeFilter = Convert.ToString(Request["consignee"]).ToLower().Trim();
                var ConsigneeTelFilter = Convert.ToString(Request["consigneetel"]).ToLower().Trim();
                var CSFilter = Convert.ToString(Request["cs"]).ToLower().Trim();
                var CSQQFilter = Convert.ToString(Request["csqq"]).ToLower().Trim();
                var EmailFilter = Convert.ToString(Request["email"]).ToLower().Trim();
                var ManagerFilter = Convert.ToString(Request["manager"]).ToLower().Trim();
                var ManagerPhoneFilter = Convert.ToString(Request["managerphone"]).ToLower().Trim();


                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.AgentName.ToLower().Contains(NameFilter));
                }

                var isCodeSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());


                if (isCodeSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.AgentCode.ToLower().Contains(CodeFilter));
                }

                var isConsigeeSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                if (isConsigeeSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.Consignee.ToLower().Contains(ConsigneeFilter));
                }

                var isConsigeeTelearchable = Convert.ToBoolean(Request["columns[4][searchable]"].ToString());

                if (isConsigeeTelearchable)
                {
                    filterAgents = filterAgents.Where(c => c.ConsigneeTel.ToLower().Contains(ConsigneeTelFilter));
                }

                var isCSSearchable = Convert.ToBoolean(Request["columns[5][searchable]"].ToString());

                if (isCSSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.CS.ToLower().Contains(CSFilter));
                }

                var isCSQQSearchable = Convert.ToBoolean(Request["columns[6][searchable]"].ToString());

                if (isCSQQSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.CSQQ.ToLower().Contains(CSQQFilter));
                }


                var isEmailSearchable = Convert.ToBoolean(Request["columns[7][searchable]"].ToString());

                if (isEmailSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.Email.ToLower().Contains(EmailFilter));
                }

                var isMangerSearchable = Convert.ToBoolean(Request["columns[7][searchable]"].ToString());

                if (isMangerSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.Email.ToLower().Contains(ManagerFilter));
                }

                var isManagerPhoneSearchable = Convert.ToBoolean(Request["columns[7][searchable]"].ToString());

                if (isManagerPhoneSearchable)
                {
                    filterAgents = filterAgents.Where(c => c.Email.ToLower().Contains(ManagerPhoneFilter));
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterAgents = allAgents;
            }
            else
            {
                filterAgents = allAgents;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isCodeFormSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isConSigneeSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
            var isConSigneeTelSortable = Convert.ToBoolean(Request["columns[4][orderable]"]);
            var isCsSortable = Convert.ToBoolean(Request["columns[5][orderable]"]);
            var isCsQQSortable = Convert.ToBoolean(Request["columns[6][orderable]"]);
            var isEmailSortable = Convert.ToBoolean(Request["columns[7][orderable]"]);
            var isManagerSortable = Convert.ToBoolean(Request["columns[8][orderable]"]);
            var isManagerPhoneSortable = Convert.ToBoolean(Request["columns[9][orderable]"]);

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Agent, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.AgentName :
                                                sortColumnIndex == 2 && isCodeFormSortable ? c.AgentCode :
                                                sortColumnIndex == 3 && isConSigneeSortable ? c.Consignee :
                                                sortColumnIndex == 4 && isConSigneeTelSortable ? c.ConsigneeTel :
                                                sortColumnIndex == 5 && isCsSortable ? c.CS:
                                                sortColumnIndex == 6 && isCsQQSortable ? c.CSQQ :
                                                sortColumnIndex == 7 && isEmailSortable ? c.Email :                                            
                                                sortColumnIndex == 8 && isManagerSortable ? c.SaleManager :
                                                sortColumnIndex == 9 && isManagerPhoneSortable ? c.SaleMangerMobile :
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterAgents = filterAgents.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterAgents = filterAgents.OrderByDescending(orderingFunction);
            }

            #endregion

            var displayedSuppliers = filterAgents.Skip(request.start).Take(request.length);
            var result = from c in displayedSuppliers
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.AgentName
                                            , c.AgentCode                                            
                                            , c.Consignee//StringUtil.CutString(StringUtil.RemoveHtml(c.Content), 100)
                                            ,c.ConsigneeTel
                                            ,c.CS
                                            ,c.CSQQ
                                            ,c.Email
                                            ,c.SaleManager
                                            ,c.SaleMangerMobile
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allAgents.Count(),
                recordsFiltered = filterAgents.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Ims/Agent/
        public ActionResult Index(AgentRequest request)
        {
            var result = this.ImsService.GetAgentList(request);
            return View(result);
        }

        public ActionResult Create()
        {
            var model = new Agent();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Agent() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<Agent>(model);
           
            try
            {
                if (ModelState.IsValid)
                { 
                    this.ImsService.SaveAgent(model);
                }
                string url = Url.Action("Index", "Agent");
                return Json(new { success = true, url = url });

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return PartialView("Edit", model);
            }

        }


        public ActionResult Edit(int id)
        {

            var model = this.ImsService.GetAgent(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.ImsService.GetAgent(id);
            this.TryUpdateModel<Agent>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.ImsService.SaveAgent(model);
                }
                string url = Url.Action("Index", "Agent");
                return Json(new { success = true, url = url });

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return PartialView("Edit", model);
            }
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteAgent(ids);
            return RedirectToAction("Index");
        }



    }
}