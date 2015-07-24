using Evious.Account.Contract;
using Evious.Crm.Contract;
using Evious.Framework.Contract;
using Evious.Framework.Utility;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Crm.Controllers
{
    [Permission(EnumBusinessPermission.CrmManage_Customer)]
    public class CustomerController : AdminControllerBase
    {

        public ActionResult Datatable(CustomerRequest request)
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
                                this.CrmService.DeleteCustomer(ids);
                                break;
                        }
                        break;

                    case "delete":
                        this.CrmService.DeleteCustomer(ids);
                        break;
                }
            }

            #endregion

            var allCustomer = this.CrmService.GetCustomerList(null);

            IEnumerable<Customer> filterCustomer = allCustomer;
            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).ToLower().Trim();

                var NumberFilter = Convert.ToString(Request["number"]).ToLower().Trim();

                var TelFilter = Convert.ToString(Request["tel"]).ToLower().Trim();

                var GenderFilter = Request["gender"];

                var CategoryFilter = Request["category"];

                var AgeGroupFilter = Request["agegroup"];

                var ProfessionFilter = Request["profession"];

                var UsernameFilter = Convert.ToString(Request["username"]).ToLower().Trim();

                var CreateTimeFilter = Convert.ToString(Request["createtime"]).Trim().ToLower();

                var CountFilter = Request["count"];

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
                var isNnmberSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());

                if (isNnmberSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Number.ToLower().Contains(NumberFilter));
                }

                var isTelSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                if (isTelSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Tel.ToLower().Contains(TelFilter));
                }



                var isGenderSearchable = false;

                if (!string.IsNullOrEmpty(GenderFilter))
                { 
                 isGenderSearchable = Convert.ToBoolean(Request["columns[4][searchable]"].ToString());
                }

                if (isGenderSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Gender == GenderFilter.ToInt());//Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumGender)c.Gender).ToLower().Contains(GenderFilter));
                }


                var isCategorySearchable = false;

                if (!string.IsNullOrEmpty(CategoryFilter))
                { 
                  isCategorySearchable = Convert.ToBoolean(Request["columns[5][searchable]"].ToString());
                }

                if (isCategorySearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Category == CategoryFilter.ToInt());
                }

                var isAgeGroupSearchable = false;

                if (!string.IsNullOrEmpty(AgeGroupFilter))
                { 
                  isAgeGroupSearchable = Convert.ToBoolean(Request["columns[6][searchable]"].ToString());
                }

                if (isAgeGroupSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.AgeGroup==AgeGroupFilter.ToInt());
                }
                var isProfessionSearchable = false;

                if (!string.IsNullOrEmpty(ProfessionFilter))
                {
                    isProfessionSearchable = Convert.ToBoolean(Request["columns[7][searchable]"].ToString());
                }

                if (isProfessionSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Profession == ProfessionFilter.ToInt());
                }

                var isUsernameSearchable = Convert.ToBoolean(Request["columns[8][searchable]"].ToString());

                if (isUsernameSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.Username.ToLower().Contains(UsernameFilter));
                }

                var isCreateTimeSearchable = Convert.ToBoolean(Request["columns[9][searchable]"].ToString());

                if (isCreateTimeSearchable)
                {
                    filterCustomer = filterCustomer.Where(c => c.CreateTime.ToCnDataString().ToLower().Contains(CreateTimeFilter));
                }

                var isCountSearchable = Convert.ToBoolean(Request["columns[10][searchable]"].ToString());

                if (isCountSearchable && !string.IsNullOrEmpty(CountFilter))
                {
                   filterCustomer = filterCustomer.Where(c => c.VisitRecords.Count==CountFilter.ToInt());
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterCustomer = allCustomer;
            }
            else
            {
                filterCustomer = allCustomer;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isNumberSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isTelSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
            var isGenderSortable = Convert.ToBoolean(Request["columns[4][orderable]"]);
            var isCategorySortable = Convert.ToBoolean(Request["columns[5][orderable]"]);
            var isAgeGroupSortable = Convert.ToBoolean(Request["columns[6][orderable]"]);
            var isProfessionSortable = Convert.ToBoolean(Request["columns[7][orderable]"]);
            var isUsernameSortable = Convert.ToBoolean(Request["columns[8][orderable]"]);
            var isCreateTimeSortable = Convert.ToBoolean(Request["columns[9][orderable]"]);
            var isVisitRecordsSortable = Convert.ToBoolean(Request["columns[10][orderable]"]);
            


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Customer, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                sortColumnIndex == 2 && isNumberSortable ? c.Number :
                                                sortColumnIndex == 3 && isTelSortable ? c.Tel :
                                                sortColumnIndex == 4 && isGenderSortable ? Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumGender)c.Gender) :
                                                sortColumnIndex == 5 && isCategorySortable ? Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumCategory)c.Category) :
                                                sortColumnIndex == 6 && isAgeGroupSortable?Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumAgeGroup)c.AgeGroup) :
                                                sortColumnIndex == 7 && isProfessionSortable ? Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumProfession)c.Profession) :
                                                sortColumnIndex == 8 && isUsernameSortable ? c.Username :
                                                sortColumnIndex == 9 && isCreateTimeSortable ? c.CreateTime.ToCnDataString() :
                                                sortColumnIndex == 10 && isVisitRecordsSortable ? c.VisitRecords.Count.ToString() :
                                                
                                                "");

            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterCustomer = filterCustomer.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterCustomer = filterCustomer.OrderByDescending(orderingFunction);
            }

            #endregion
            var displayedCustomer = filterCustomer.Skip(request.start).Take(request.length);
            var result = from c in displayedCustomer
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.Number
                                            , c.Tel
                                            , Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumGender)c.Gender)
                                            , Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumCategory)c.Category)
                                            , Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumAgeGroup)c.AgeGroup)
                                            , Evious.Framework.Utility.EnumHelper.GetEnumTitle((EnumProfession)c.Profession)
                                            , c.Username
                                            ,c.CreateTime.ToCnDataString()
                                            ,c.VisitRecords.Count.ToString()                                            
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allCustomer.Count(),//alltransactions.Count(),
                recordsFiltered = filterCustomer.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Crm/Customer/

        public ActionResult Index(CustomerRequest request)
        {
            this.TryUpdateModel<Customer>(request.Customer);

            this.ModelState.Clear();

            this.RenderMyViewData(request.Customer, true);

            var result = this.CrmService.GetCustomerList(request);
            return View(result);
        }

        //
        // GET: /Crm/Customer/Create

        public ActionResult Create()
        {
            var model = new Customer();

            this.RenderMyViewData(model);

            return View("Edit", model);
        }

        //
        // POST: /Crm/Customer/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Customer();
            this.TryUpdateModel<Customer>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.CrmService.SaveCustomer(model);
                }
                string url = Url.Action("Index", "Customer");
                return Json(new { success = true, url = url });    
            }

            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.RenderMyViewData(model);
                return View("Edit", model);
            }
        }

        //
        // GET: /Crm/Customer/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.CrmService.GetCustomer(id);
            this.RenderMyViewData(model);
            return View(model);
        }

        //
        // POST: /Crm/Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.CrmService.GetCustomer(id);
            this.TryUpdateModel<Customer>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.CrmService.SaveCustomer(model);
                }
                string url = Url.Action("Index", "Customer");
                return Json(new { success = true, url = url });  


            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.RenderMyViewData(model);
                return View("Edit", model);
            }

            
        }

        // POST: /Crm/Customer/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.CrmService.DeleteCustomer(ids);
            return RedirectToAction("Index");
        }

        private void RenderMyViewData(Customer model, bool isBasic = false)
        {
            ViewData.Add("Gender", new SelectList(EnumHelper.GetItemValueList<EnumGender>(), "Key", "Value", model.Gender));
            ViewData.Add("Category", new SelectList(EnumHelper.GetItemValueList<EnumCategory>(), "Key", "Value", model.Category));
            ViewData.Add("Profession", new SelectList(EnumHelper.GetItemValueList<EnumProfession>(), "Key", "Value", model.Profession));
            ViewData.Add("AgeGroup", new SelectList(EnumHelper.GetItemValueList<EnumAgeGroup>(), "Key", "Value", model.AgeGroup));

            if (isBasic)
                return;

            ViewData.Add("UserId", new SelectList(this.AccountService.GetUserList(), "ID", "LoginName", model.UserId));
        }
    }
}