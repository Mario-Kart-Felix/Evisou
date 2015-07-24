using Evious.Account.Contract;
using Evious.Framework.Contract;
using Evious.Ims.Contract.Model;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_PaypalApi)]
    public class PaypalApiController : AdminControllerBase
    {
        public ActionResult Datatable(PaypalApiRequest request)
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
                                this.ImsService.DeletePaypalApi(ids);
                                break;

                        }
                        break;

                    case "delete":
                        this.ImsService.DeletePaypalApi(ids);
                        break;
                }
            }
            #endregion


            var allPaypalApi = this.ImsService.GetPaypalApiList(null);

            IEnumerable<PaypalApi> filterPaypalApi = allPaypalApi;

            #region 搜索
            //if (!string.IsNullOrEmpty(request.search))
            //{
            //    var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            //}
            //else if (request.action == "filter")
            //{
            //    var NameFilter = Convert.ToString(Request["name"]).Trim().ToLower();


            //    var SkuFilter = Convert.ToString(Request["sku"]).Trim().ToLower();

            //    var PackingWeightFilter = Convert.ToString(Request["packingweight"]).Trim().ToLower();

            //    var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

            //    if (isNameSearchable)
            //    {
            //        filterProduct = filterProduct.Where(c => c.Name.ToLower().Contains(NameFilter));
            //    }

            //    var isSkuSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());


            //    if (isSkuSearchable)
            //    {
            //        filterProduct = filterProduct.Where(c => c.Sku.ToLower().Contains(SkuFilter));
            //    }

            //    var isPackingWeightSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

            //    if (isPackingWeightSearchable && !string.IsNullOrEmpty(PackingWeightFilter))
            //    {
            //        filterProduct = filterProduct.Where(c => c.PackWeight == PackingWeightFilter.ToInt());
            //    }

            //}
            //else if (request.action == "filter_cancel")
            //{
            //    filterProduct = allProduct;
            //}
            //else
            //{
            //    filterProduct = allProduct;
            //}
            #endregion

            #region 排序
            var isPPAccountSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isApiUserNameSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);            
            var isIsActiveSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<PaypalApi, string> orderingFunction = (c =>
                                               sortColumnIndex == 1 && isPPAccountSortable ?c.PPAccount :
                                               sortColumnIndex == 2 && isApiUserNameSortable ? c.ApiUserName :     
                                               sortColumnIndex==3&& isIsActiveSortable?c.IsActive.ToString():
                                                "");

            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterPaypalApi = filterPaypalApi.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterPaypalApi = filterPaypalApi.OrderByDescending(orderingFunction);
            }

            #endregion


            var displayedPaypalApi = filterPaypalApi.Skip(request.start).Take(request.length);
            var result = from c in displayedPaypalApi
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            ,c.PPAccount//c.OrderTime.ToCnDataString()
                                            , c.ApiUserName//c.TransactionID                                           
                                            , c.IsActive.ToString()                                                                             
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allPaypalApi.Count(),//alltransactions.Count(),
                recordsFiltered = filterPaypalApi.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Ims/PaypalApi/
        public ActionResult Index(PaypalApiRequest request=null)
        {
            var result = this.ImsService.GetPaypalApiList(request);
            return View(result);
        }


        public ActionResult Create()
        {
            var model = new PaypalApi();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new PaypalApi() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<PaypalApi>(model);
           
            try
            {
                if (ModelState.IsValid)
                {
                    this.ImsService.SavePaypalApi(model);
                    string url = Url.Action("Index", "PaypalApi");
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


        public ActionResult Edit(int id)
        {

            var model = this.ImsService.GetPaypalApi(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.ImsService.GetPaypalApi(id);
            this.TryUpdateModel<PaypalApi>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.ImsService.SavePaypalApi(model);
                    string url = Url.Action("Index", "PaypalApi");
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

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeletePaypalApi(ids);
            return RedirectToAction("Index");
        }
    }
}