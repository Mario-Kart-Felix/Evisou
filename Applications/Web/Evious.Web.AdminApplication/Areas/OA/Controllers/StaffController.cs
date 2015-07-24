using Evious.Account.Contract;
using Evious.OA.Contract;
using Evious.Web.AdminApplication.Common;
using Evious.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evious.Framework.Contract;

namespace Evious.Web.AdminApplication.Areas.OA.Controllers
{
    [Permission(EnumBusinessPermission.OAManage_Staff)]
    public class StaffController : AdminControllerBase
    {

        public ActionResult Datatable(StaffRequest request)
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
                                this.OAService.DeleteStaff(ids);
                                break;
                        }
                        break;

                    case "delete":
                        this.OAService.DeleteStaff(ids);
                        break;
                }
            }

            #endregion

            var allStaff = this.OAService.GetStaffList(null);

            IEnumerable<Staff> filterStaff = allStaff;


            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).ToLower().Trim();

                var PositionFilter = Convert.ToString(Request["position"]).ToLower().Trim();

                var BranchFilter = Convert.ToString(Request["branchId"]).ToLower().Trim();

                var GenderFilter = Convert.ToString(Request["gender"]).ToLower().Trim();

                var BrithFromFilter = Convert.ToString(Request["brith_from"]).ToLower().Trim();

                var BrithToFilter = Convert.ToString(Request["brith_to"]).ToLower().Trim();

                var EmailFilter = Convert.ToString(Request["email"]).ToLower().Trim();

                var TelFilter = Convert.ToString(Request["tel"]).ToLower().Trim();

               // var AddressFilter = Convert.ToString(Request["address"]).Trim().ToLower();

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterStaff = filterStaff.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
                var isPositionSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());

                if (isPositionSearchable && !string.IsNullOrEmpty(PositionFilter))
                {
                    filterStaff = filterStaff.Where(c => c.Position==PositionFilter.ToInt());
                }

                var isBranchSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                if (isBranchSearchable)
                {
                  //  filterStaff = filterStaff.Where(c => c.Branch != null && c.Branch.ID == BranchFilter.ToInt());
                }

                var isGenderSearchable = Convert.ToBoolean(Request["columns[4][searchable]"].ToString());

                if (isGenderSearchable)
                {
                    filterStaff = filterStaff.Where(c => Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumGender)c.Gender).ToLower().Contains(GenderFilter));
                }

                var isBirthSearchable = Convert.ToBoolean(Request["columns[5][searchable]"].ToString());

                if (isBirthSearchable && !string.IsNullOrEmpty(BrithFromFilter) && !string.IsNullOrEmpty(BrithToFilter))
                {
                    filterStaff = filterStaff.Where(c => c.BirthDate >= DateTime.Parse(BrithFromFilter) && c.BirthDate <= DateTime.Parse(BrithToFilter));
                }

                var isEmailSearchable = Convert.ToBoolean(Request["columns[6][searchable]"].ToString());

                if (isEmailSearchable)
                {
                    filterStaff = filterStaff.Where(c =>c.Email.ToLower().Contains(EmailFilter));
                }

                var isTelSearchable = Convert.ToBoolean(Request["columns[7][searchable]"].ToString());

                if (isTelSearchable)
                {
                    filterStaff = filterStaff.Where(c => c.Tel.ToLower().Contains(TelFilter));
                }

                var isAddressSearchable = Convert.ToBoolean(Request["columns[8][searchable]"].ToString());

                if (isAddressSearchable)
                {
                   // filterStaff = filterStaff.Where(c => c.Address.ToLower().Contains(AddressFilter));
                    //filterStaff = filterStaff.Where(c => (!string.IsNullOrEmpty(c.Address))).Where(c => c.Address.ToLower().Contains(AddressFilter));
                }

               



            }
            else if (request.action == "filter_cancel")
            {
                filterStaff = allStaff;
            }
            else
            {
                filterStaff = allStaff;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isPositionSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isBranchSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
            var isGenderSortable = Convert.ToBoolean(Request["columns[4][orderable]"]);
            var isBirthSortable = Convert.ToBoolean(Request["columns[5][orderable]"]);
            var isEmailSortable = Convert.ToBoolean(Request["columns[6][orderable]"]);
            var isTelSortable = Convert.ToBoolean(Request["columns[7][orderable]"]);
            var isAddressSortable = Convert.ToBoolean(Request["columns[8][orderable]"]);


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Staff, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                sortColumnIndex == 2 && isPositionSortable ? Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumPosition)c.Position):
                                                sortColumnIndex == 3 && isBranchSortable && c.Branch != null ? c.Branch.Name :
                                                sortColumnIndex == 4 && isGenderSortable ? Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumGender)c.Gender) :
                                                sortColumnIndex == 5 && isBirthSortable ? c.BirthDate.ToCnDataString() :
                                                sortColumnIndex == 6 && isEmailSortable ? c.Email :
                                                sortColumnIndex == 7 && isTelSortable ? c.Tel :
                                                sortColumnIndex == 8 && isAddressSortable ? c.Address:
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterStaff = filterStaff.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterStaff = filterStaff.OrderByDescending(orderingFunction);
            }

            #endregion
            var displayedStaff = filterStaff.Skip(request.start).Take(request.length);
            var result = from c in displayedStaff
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumPosition)c.Position)
                                            , c.Branch == null ? "未分配" : c.Branch.Name
                                            , Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumGender)c.Gender)
                                            , c.BirthDate.ToCnDataString()
                                            , c.Email
                                            , c.Tel
                                            , c.Address
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allStaff.Count(),//alltransactions.Count(),
                recordsFiltered = filterStaff.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /OA/Staff/

        public ActionResult Index(StaffRequest request)
        {
            var branchList = this.OAService.GetBranchList(null);
            this.ViewBag.branchId = new SelectList(branchList, "ID", "Name");

            var roleList = this.AccountService.GetRoleList(null);
            this.ViewBag.roleId = new SelectList(Evious.Framework.Utility.EnumHelper.GetAllItemList<EnumPosition>());//new SelectList(roleList, "ID", "Name");

            this.ViewBag.position = new SelectList(EnumHelper.GetItemValueList<EnumPosition>(), "Key", "Value");
            var result = this.OAService.GetStaffList(request);
            return View(result);
        }

        //
        // GET: /OA/Staff/Create

        public ActionResult Create()
        {
            var model = new Staff() { };
            this.RenderMyViewData(model);
            return View("Edit", model);
        }

        //
        // POST: /OA/Staff/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            var model = new Staff();
            this.TryUpdateModel<Staff>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.OAService.SaveStaff(model);
                    string url = Url.Action("Index", "staff");
                    return Json(new { success = true, url = url });
                }
                this.RenderMyViewData(model);
                return PartialView("Edit", model);

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);           
                return PartialView("Edit", model);
            }

            
        }

        //
        // GET: /OA/Staff/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.OAService.GetStaff(id);
            this.RenderMyViewData(model);
            return View(model);
        }

        //
        // POST: /OA/Staff/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.OAService.GetStaff(id);
            this.TryUpdateModel<Staff>(model);
            try
            {
                if (ModelState.IsValid)
                {
                    this.OAService.SaveStaff(model);
                    string url = Url.Action("Index", "staff");
                    return Json(new { success = true, url = url });
                }
                this.RenderMyViewData(model);
                return PartialView("Edit", model);

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);                
                return PartialView("Edit", model);
            }          

            
        }

        // POST: /OA/Staff/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.OAService.DeleteStaff(ids);
            return RedirectToAction("Index");
        }

        private void RenderMyViewData(Staff model)
        {
            ViewData.Add("Position", new SelectList(EnumHelper.GetItemValueList<EnumPosition>(), "Key", "Value", model.Position));
            ViewData.Add("Gender", new SelectList(EnumHelper.GetItemValueList<EnumGender>(), "Key", "Value", model.Gender));
        }
    }
}