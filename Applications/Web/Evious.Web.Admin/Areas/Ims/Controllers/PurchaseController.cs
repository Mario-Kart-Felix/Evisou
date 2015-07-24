using Evious.Account.Contract;
using Evious.Ims.Contract.Model;
using Evious.Web.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.Admin.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Purchase)]
    public class PurchaseController : AdminControllerBase
    {
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
            this.ImsService.SavePurchase(model);
            return this.RefreshParent();
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

            this.ImsService.SavePurchase(model);

            return this.RefreshParent();
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

	}
}