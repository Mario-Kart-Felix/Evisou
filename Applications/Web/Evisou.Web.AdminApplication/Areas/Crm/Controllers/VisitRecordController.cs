using Evisou.Account.Contract;
using Evisou.Crm.Contract;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Crm.Controllers
{
   [Permission(EnumBusinessPermission.CrmManage_VisitRecord)]
    public class VisitRecordController : AdminControllerBase
    {

       public ActionResult Datatable(VisitRecordRequest request)
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
                                this.CrmService.DeleteVisitRecord(ids);
                                break;
                        }
                        break;

                    case "delete":
                        this.CrmService.DeleteVisitRecord(ids);
                        break;
                }
            }

            #endregion

           var allVisitRecord = this.CrmService.GetVisitRecordList(null);

           IEnumerable<VisitRecord> filterVisitRecord = allVisitRecord;
           #region 排序
           var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
           var isNumberSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
           var isTelSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
           var isVisitWaySortable = Convert.ToBoolean(Request["columns[4][orderable]"]);
           var isCitySortable = Convert.ToBoolean(Request["columns[5][orderable]"]);
           var isProjectNameSortable = Convert.ToBoolean(Request["columns[6][orderable]"]);
           var isMotivationSortable = Convert.ToBoolean(Request["columns[7][orderable]"]);
           var isAreaDemandSortable = Convert.ToBoolean(Request["columns[8][orderable]"]);
           var isPriceResponseSortable = Convert.ToBoolean(Request["columns[9][orderable]"]);
           var isCognitiveChannelSortable = Convert.ToBoolean(Request["columns[10][orderable]"]);
           var isFocusSortable = Convert.ToBoolean(Request["columns[11][orderable]"]);
           var isUsernameSortable = Convert.ToBoolean(Request["columns[12][orderable]"]);
           var isVisitTimeSortable = Convert.ToBoolean(Request["columns[13][orderable]"]);


           var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

           Func<VisitRecord, string> orderingFunction = (c =>
                                               sortColumnIndex == 1 && isNameSortable && c.Customer!=null? c.Customer.Name :
                                               sortColumnIndex == 2 && isNumberSortable && c.Customer!=null? c.Customer.Number :
                                               sortColumnIndex == 3 && isTelSortable&&c.Customer!=null? c.Customer.Tel:
                                               sortColumnIndex == 4 && isVisitWaySortable ? Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumVisitWay)c.VisitWay):
                                               sortColumnIndex == 5 && isCitySortable ? AdminCacheContext.Current.AreaDic[c.AreaId].Name :
                                               sortColumnIndex == 6 && isProjectNameSortable&&c.Customer == null ? StringUtil.CutString(c.Project.Name, 10) :
                                               sortColumnIndex == 7 && isMotivationSortable ? Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumMotivation)c.Motivation) :
                                               sortColumnIndex == 8 && isAreaDemandSortable ? Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumAreaDemand)c.AreaDemand):
                                               sortColumnIndex == 9 && isPriceResponseSortable ? Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumPriceResponse)c.PriceResponse):
                                               sortColumnIndex == 10 && isCognitiveChannelSortable ? StringUtil.CutString(Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumCognitiveChannel)c.CognitiveChannel), 20):
                                               sortColumnIndex == 11&& isFocusSortable ? StringUtil.CutString(Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumFocus)c.Focus), 20):
                                               sortColumnIndex == 12 && isUsernameSortable ? c.Username:
                                               sortColumnIndex == 13 && isVisitTimeSortable ? c.VisitTime.ToString("yyyy-MM-dd HH:mm"):
                                               "");

           var sortDirection = Request["order[0][dir]"]; // asc or desc

           if (sortDirection == "asc")
           {
               filterVisitRecord = filterVisitRecord.OrderBy(orderingFunction);
           }

           if (sortDirection == "desc")
           {
               filterVisitRecord = filterVisitRecord.OrderByDescending(orderingFunction);
           }

           #endregion
           var displayedVisitRecord = filterVisitRecord.Skip(request.start).Take(request.length);
           var result = from c in displayedVisitRecord
                        select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Customer == null ? string.Empty : c.Customer.Name
                                            , c.Customer == null ? string.Empty : c.Customer.Number
                                            , c.Customer == null ? string.Empty : c.Customer.Tel
                                            , Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumVisitWay)c.VisitWay)
                                            , AdminCacheContext.Current.AreaDic[c.AreaId].Name+AdminCacheContext.Current.CityDic[c.CityId].Name
                                            , c.Customer == null ? string.Empty : StringUtil.CutString(c.Project.Name, 10)
                                            , Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumMotivation)c.Motivation)
                                            , Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumAreaDemand)c.AreaDemand)
                                            ,Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumPriceResponse)c.PriceResponse)
                                            ,StringUtil.CutString(Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumCognitiveChannel)c.CognitiveChannel), 20)
                                            ,StringUtil.CutString(Evisou.Framework.Utility.EnumHelper.GetEnumTitle((EnumFocus)c.Focus), 20)
                                            ,c.Username
                                            ,c.VisitTime.ToString("yyyy-MM-dd HH:mm")
                                            ,Convert.ToString(c.ID)
                         
                         };
           return Json(new
           {
               draw = request.draw,//param.sEcho,
               recordsTotal = allVisitRecord.Count(),//alltransactions.Count(),
               recordsFiltered = filterVisitRecord.Count(),
               data = result
           },
                            JsonRequestBehavior.AllowGet);
       }
        //
        // GET: /Crm/VisitRecord/

        public ActionResult Index(VisitRecordRequest request)
        {
            this.TryUpdateModel<VisitRecord>(request.VisitRecord);

            this.ModelState.Clear();
            
            this.RenderMyViewData(request.VisitRecord, true);
            var areas = this.AreaDic.Values.Select(c => new { Id = c.ID, Name = c.Name + "-" + this.CityDic[c.CityId].Name });
            ViewData.Add("AreaId", new SelectList(areas, "Id", "Name", request.VisitRecord.AreaId));
            
            var result = this.CrmService.GetVisitRecordList(request);
            return View(result);
        }

        //
        // GET: /Crm/VisitRecord/Create

        public ActionResult Create()
        {
            var model = new VisitRecord();

            this.RenderMyViewData(model);

            return View("Edit", model);
        }

        //
        // POST: /Crm/VisitRecord/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new VisitRecord();
            this.TryUpdateModel<VisitRecord>(model);

            model.CreateTime = model.VisitTime = DateTime.Now;
            model.Username = this.UserContext.LoginInfo.LoginName;
            model.UserId = this.UserContext.LoginInfo.UserID;

            try
            {
                this.CrmService.SaveVisitRecord(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.RenderMyViewData(model);
                return View("Edit", model);
            }
            

            return this.RefreshParent();
        }

        //
        // GET: /Crm/VisitRecord/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.CrmService.GetVisitRecord(id);
            this.RenderMyViewData(model);
            return View(model);
        }

        //
        // POST: /Crm/VisitRecord/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.CrmService.GetVisitRecord(id);
            this.TryUpdateModel<VisitRecord>(model);

            try
            {
                this.CrmService.SaveVisitRecord(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                this.RenderMyViewData(model);
                return View("Edit", model);
            }

            return this.RefreshParent();
        }

        // POST: /Crm/VisitRecord/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.CrmService.DeleteVisitRecord(ids);
            return RedirectToAction("Index");
        }

        public ActionResult GetArea(int cityId)
        {
            var areas = this.AreaDic.Values.Where(a => a.CityId == cityId);
            ViewData.Add("AreaId", new SelectList(areas, "Id", "Name"));

            return PartialView("AreaSelect");
        }

        private void RenderMyViewData(VisitRecord model, bool isBasic = false)
        {
            ViewData.Add("VisitWay", new SelectList(EnumHelper.GetItemValueList<EnumVisitWay>(), "Key", "Value", model.VisitWay));
            ViewData.Add("FollowLevel", new SelectList(EnumHelper.GetItemValueList<EnumFollowLevel>(), "Key", "Value", model.FollowLevel));
            ViewData.Add("FollowStep", new SelectList(EnumHelper.GetItemValueList<EnumFollowStep>(), "Key", "Value", model.FollowStep));
            ViewData.Add("ProductType", new SelectList(EnumHelper.GetItemValueList<EnumProductType>(), "Key", "Value", model.ProductType));

            ViewData.Add("Focus", new SelectList(EnumHelper.GetItemList<EnumFocus>(), "Key", "Value", (EnumFocus)model.Focus));
            ViewData.Add("CognitiveChannel", new SelectList(EnumHelper.GetItemList<EnumCognitiveChannel>(), "Key", "Value", (EnumCognitiveChannel)model.CognitiveChannel));
            ViewData.Add("PriceResponse", new SelectList(EnumHelper.GetItemValueList<EnumPriceResponse>(), "Key", "Value", model.PriceResponse));
            ViewData.Add("AreaDemand", new SelectList(EnumHelper.GetItemValueList<EnumAreaDemand>(), "Key", "Value", model.AreaDemand));
            ViewData.Add("Motivation", new SelectList(EnumHelper.GetItemValueList<EnumMotivation>(), "Key", "Value", model.Motivation));

            ViewData.Add("ProjectId", new SelectList(this.CrmService.GetProjectList(), "Id", "Name", model.ProjectId));

            if (isBasic)
                return;

            ViewData.Add("CityId", new SelectList(this.CityDic.Values, "Id", "Name", model.CityId));

            if (model.CityId == 0)
                model.CityId = this.CityDic.First().Key;

            var areas = this.AreaDic.Values.Where(a => a.CityId == model.CityId);
            ViewData.Add("AreaId", new SelectList(areas, "Id", "Name", model.AreaId));

            var request = new CustomerRequest();
            request.Customer.UserId = this.UserContext.LoginInfo.UserID;
            var customerList = this.CrmService.GetCustomerList(request).ToList();
            customerList.ForEach(c => c.Name = string.Format("{0}({1})", c.Name, c.Tel));
            ViewData.Add("CustomerId", new SelectList(customerList, "Id", "Name", model.CustomerId));

        }

        public Dictionary<int, City> CityDic
        {
            get
            {
                return AdminCacheContext.Current.CityDic;
            }
        }

        public Dictionary<int, Area> AreaDic
        {
            get
            {
                return AdminCacheContext.Current.AreaDic;
            }
        }
    }
}
