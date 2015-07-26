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
    [Permission(EnumBusinessPermission.ImsManage_Supplier)]
    public class SupplierController : AdminControllerBase
    {
        //
        // GET: /Ims/Supplier/
        public ActionResult Index(SupplierRequest request)
        {
            var result = this.ImsService.GetSupplierList(request);
            return View(result);
        }

        public ActionResult Create()
        {
            /*var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
            this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name");
            this.ViewBag.Tags = this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits });*/
            ViewBag.Products = this.ImsService.GetProductList(null);
            var model = new Supplier();
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Supplier() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<Supplier>(model);

            this.ImsService.SaveSupplier(model);

            return this.RefreshParent();
        }


        public ActionResult Edit(int id)
        {
            ViewBag.Products = this.ImsService.GetProductList(null);
            var model = this.ImsService.GetSupplier(id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (collection["SelectedProduct"] == null)
            {
                collection.Add("SelectedProduct", null);
            }
            //collection["SelectedProduct"] == null ? collection.Add("SelectedProduct", null) : collection;
            var model = this.ImsService.GetSupplier(id);
            this.TryUpdateModel<Supplier>(model,collection);

            this.ImsService.SaveSupplier(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteSupplier(ids);
            return RedirectToAction("Index");
        }
	}
}