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
    [Permission(EnumBusinessPermission.ImsManage_Agent)]
    public class AgentController :  AdminControllerBase
    {
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
            this.ImsService.SaveAgent(model);
            return this.RefreshParent();
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

            this.ImsService.SaveAgent(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteAgent(ids);
            return RedirectToAction("Index");
        }

       

	}
}