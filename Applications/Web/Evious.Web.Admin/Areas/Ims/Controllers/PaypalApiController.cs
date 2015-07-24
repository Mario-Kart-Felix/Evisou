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
    [Permission(EnumBusinessPermission.ImsManage_PaypalApi)]
    public class PaypalApiController : AdminControllerBase
    {
        //
        // GET: /Ims/PaypalApi/
        public ActionResult Index(PaypalApiRequest request)
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
            this.ImsService.SavePaypalApi(model);
            return this.RefreshParent();
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

            this.ImsService.SavePaypalApi(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeletePaypalApi(ids);
            return RedirectToAction("Index");
        }
	}
}