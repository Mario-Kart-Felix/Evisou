using Evious.Account.Contract;
using Evious.Ims.Contract.Model;
using Evious.Web.Admin.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Evious.Web.Admin.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Product)]
    public class ProductController : AdminControllerBase
    {
        //
        // GET: /Pms/Product/
        public ActionResult Index(ProductRequest request)
        {
            var result = this.ImsService.GetProductList(request);
            return View(result);

        }

        public ActionResult Create()
        {
            var model = new Product();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Product() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<Product>(model);

            this.ImsService.SaveProduct(model);

            return this.RefreshParent();
        }

        public ActionResult Edit(int id)
        {

            var model = this.ImsService.GetProduct(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.ImsService.GetProduct(id);
            this.TryUpdateModel<Product>(model);

            this.ImsService.SaveProduct(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteProduct(ids);
            return RedirectToAction("Index");
        }
    }
}