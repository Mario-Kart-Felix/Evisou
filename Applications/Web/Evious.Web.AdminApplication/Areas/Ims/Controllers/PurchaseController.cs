using Evious.Account.Contract;
using Evious.Ims.Contract.Model;
using Evious.Framework.Utility;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evious.Framework.Contract;

namespace Evious.Web.AdminApplication.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Purchase)]
    public class PurchaseController : AdminControllerBase
    {


        public ActionResult Datatable(PurchaseRequest request)
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
                                // DeleteImageFile(id_Array);
                                this.ImsService.DeletePurchase(ids);
                                break;

                        }
                        break;

                    case "delete":
                        // DeleteImageFile(id_Array);
                        this.ImsService.DeletePurchase(ids);
                        break;
                }
            }
            #endregion


            var allPurchase = this.ImsService.GetPurchaseList(null);

            IEnumerable<Purchase> filterPurchase = allPurchase;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var PurchaseDateFilter = Convert.ToString(Request["purchasedate"]).Trim().ToLower();


                var PurchaseTransactionIDFilter = Convert.ToString(Request["purchasetransactionid"]).Trim().ToLower();



                var isPurchaseDateSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isPurchaseDateSearchable)
                {
                   // filterPurchase = filterPurchase.Where(c => c.Sku.ToLower().Contains(SkuFilter));
                }

                var isPurchaseTransactionIDSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());

                if (isPurchaseTransactionIDSearchable)
                {
                    filterPurchase = filterPurchase.Where(c => c.PurchaseTransactionID.ToLower().Contains(PurchaseTransactionIDFilter));
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterPurchase = allPurchase;
            }
            else
            {
                filterPurchase = allPurchase;
            }
            #endregion

            #region 排序
            var isPurchaseDateSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isPurchaseTransactionIDSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
           


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Purchase, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isPurchaseDateSortable ? c.PurchaseDate.ToCnDataString() :
                                                sortColumnIndex == 2 && isPurchaseTransactionIDSortable ? c.PurchaseTransactionID :                                                
                                                "");

            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterPurchase = filterPurchase.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterPurchase = filterPurchase.OrderByDescending(orderingFunction);
            }

            #endregion
            var displayedPurchase = filterPurchase.Skip(request.start).Take(request.length);
            var result = from c in displayedPurchase
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.PurchaseDate.ToCnDataString()
                                             , c.PurchaseTransactionID
                                            // , c.PurchaseTransactionID.Substring(0,4)
                                            //+"-"+c.PurchaseTransactionID.Substring(4,2)
                                            //+"-"+c.PurchaseTransactionID.Substring(6,2)
                                            //+"-"+c.PurchaseTransactionID.Substring(8,4)
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allPurchase.Count(),//alltransactions.Count(),
                recordsFiltered = filterPurchase.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);

        }

        //
        // GET: /Ims/Purchase/
        public ActionResult Index(PurchaseRequest request)
        {
            var result = this.ImsService.GetPurchaseList(request);
            return View(result);
        }

        public ActionResult Create()
        {
            var supplierList = this.ImsService.GetSupplierList(new SupplierRequest() { });
            this.ViewBag.Supplier = new SelectList(supplierList, "ID", "Name");
            var model = new Purchase();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Purchase() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };


            this.TryUpdateModel<Purchase>(model);   
           // string[] inclubproi={"PurchaseDate","PurchaseTransactionID"};
            //this.TryUpdateModel(model, inclubproi);
            try
            {
                if (ModelState.IsValid)
                {
                    this.ImsService.SavePurchase(model);

                }
                string url = Url.Action("Index", "Purchase");
                return Json(new { success = true, url = url });

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                var supplierList = this.ImsService.GetSupplierList(new SupplierRequest() { });
                this.ViewBag.Supplier = new SelectList(supplierList, "ID", "Name");
                return PartialView("Edit", model);
            }

        }

        public ActionResult Edit(int id)
        {
            var supplierList = this.ImsService.GetSupplierList(new SupplierRequest() { });
            this.ViewBag.Supplier = new SelectList(supplierList, "ID", "Name");
            var model = this.ImsService.GetPurchase(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.ImsService.GetPurchase(id);
            this.TryUpdateModel<Purchase>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.ImsService.SavePurchase(model);

                }
                string url = Url.Action("Index", "Purchase");
                return Json(new { success = true, url = url });

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                var supplierList = this.ImsService.GetSupplierList(new SupplierRequest() { });
                this.ViewBag.Supplier = new SelectList(supplierList, "ID", "Name");
                return PartialView("Edit", model);
            }
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeletePurchase(ids);
            return RedirectToAction("Index");
        }

        public JsonResult GetProductBySupplier(int supplier, int? purchaseid = null)
        {

            var ret = this.ImsService.getProductBySupplier(supplier, purchaseid);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public string AjaxCheckForm(PurchaseRequest request)
        {
            if (request.ID != 0)
            {
                var purchases = this.ImsService.GetPurchaseList(request);

                int count=purchases.Count<Purchase>() ;
                string result = string.Empty;
                switch(count)
                {
                    case 0:
                        result = "true";
                        break;
                    case 1:
                        result = "true";
                        break;
                    default:
                        result = "false";
                        break;
                }
                return result;
            }else{
            var purchases = this.ImsService.GetPurchaseList(request);
            return purchases.Count<Purchase>() == 0 ? "true" : "false";
            }
            

        }

    }
}