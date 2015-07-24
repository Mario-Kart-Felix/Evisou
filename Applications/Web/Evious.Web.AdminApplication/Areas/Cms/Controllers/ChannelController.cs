using Evious.Account.Contract;
using Evious.Cms.Contract;
using Evious.Framework.Contract;
using Evious.Framework.Utility;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Cms.Controllers
{
    [Permission(EnumBusinessPermission.CmsManage_Channel)]
    public class ChannelController : AdminControllerBase
    {

        public JsonResult Datatable(ChannelRequest request)
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
                                this.CmsService.DeleteChannel(ids);
                                break;
                            case "freeze":
                                foreach (var id in ids)
                                {
                                    var model = this.CmsService.GetChannel(id);
                                    model.IsActive = false;
                                    this.TryUpdateModel<Channel>(model);
                                    this.CmsService.SaveChannel(model);
                                }
                                break;

                            case "active":
                                foreach (var id in ids)
                                {
                                    var model = this.CmsService.GetChannel(id);
                                    model.IsActive = true;
                                    this.TryUpdateModel<Channel>(model);
                                    this.CmsService.SaveChannel(model);
                                }
                                break;
                        }
                        break;

                    case "delete":
                        this.CmsService.DeleteChannel(ids);
                        break;
                }
            }

            #endregion

            var allChannel = this.CmsService.GetChannelList(null);

            IEnumerable<Channel> filterChannel = allChannel;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).Trim();

                var DescFilter = Request["desc"];

                var ActiveFilter = Convert.ToString(Request["isactive"]);

                // var isTitleSearchable = string.IsNullOrEmpty(Request["columns[1][searchable]"].ToString()) ? false : true;

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterChannel = filterChannel.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
                var isDescSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());

                if (isDescSearchable)
                {
                    filterChannel = filterChannel.Where(c => c.Desc.ToLower().Contains(DescFilter));
                }

                var isActiveNameSearchable = false;
                if (!string.IsNullOrEmpty(Request["isactive"].ToString()))
                {
                    isActiveNameSearchable = string.IsNullOrEmpty(Request["columns[3][searchable]"].ToString()) ? false : true;
                }

                if (isActiveNameSearchable)
                {
                    filterChannel = filterChannel.Where(c => c.IsActive == ActiveFilter.ToBool());
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterChannel = allChannel;
            }
            else
            {
                filterChannel = allChannel;
            }
            #endregion
            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isDescSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isActiveSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Channel, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                sortColumnIndex==2&&isDescSortable?c.Desc:
                                                sortColumnIndex==3&&isActiveSortable?c.IsActive.ToString():
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterChannel = filterChannel.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterChannel = filterChannel.OrderByDescending(orderingFunction);
            }

            #endregion

            var displayedChannel = filterChannel.Skip(request.start).Take(request.length);
            var result = from c in displayedChannel
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.Desc                                            
                                            , c.IsActive.ToString()
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allChannel.Count(),//alltransactions.Count(),
                recordsFiltered = filterChannel.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Cms/Channel/

        public ActionResult Index(ChannelRequest request)
        {
            var result = this.CmsService.GetChannelList(request);
            return View(result);
        }

        //
        // GET: /Cms/Channel/Create

        public ActionResult Create()
        {
            var model = new Channel() { IsActive = true };
            return View("Edit", model);
        }

        //
        // POST: /Cms/Channel/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Channel();
            this.TryUpdateModel<Channel>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.CmsService.SaveChannel(model);                  
                    string url = Url.Action("Index", "Channel");
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

        //
        // GET: /Cms/Channel/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.CmsService.GetChannel(id);
            return View(model);
        }

        //
        // POST: /Cms/Channel/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.CmsService.GetChannel(id);
            this.TryUpdateModel<Channel>(model);

            try
            {
                if (ModelState.IsValid)
                {
                    this.CmsService.SaveChannel(model);
                    string url = Url.Action("Index", "Channel");
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

        // POST: /Cms/Channel/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.CmsService.DeleteChannel(ids);
            return RedirectToAction("Index");
        }
    }
}