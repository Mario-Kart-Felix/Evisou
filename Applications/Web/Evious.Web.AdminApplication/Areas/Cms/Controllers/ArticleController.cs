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
    public class ArticleController : AdminControllerBase
    {

        public JsonResult Datatable(ArticleRequest request)
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
                                this.CmsService.DeleteArticle(ids);
                                break;
                           
                        }
                        break;

                    case "delete":
                        this.CmsService.DeleteArticle(ids);
                        break;
                }
            }
            #endregion

            var allArticles = this.CmsService.GetArticleList(null);

            IEnumerable<Article> filterArticles = allArticles;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var TitleFilter = Convert.ToString(Request["title"]).Trim();

                var ChannelFilter=Request["ChannelId"];

                var ContentFilter = Convert.ToString(Request["content"]).Trim();

               // var isTitleSearchable = string.IsNullOrEmpty(Request["columns[1][searchable]"].ToString()) ? false : true;

                var isTitleSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());              

                if (isTitleSearchable)
                {
                    filterArticles = filterArticles.Where(c => c.Title.ToLower().Contains(TitleFilter));
                }
                var isChannelSearchable = false;

                if (!string.IsNullOrEmpty(Request["ChannelId"]))
                { 
                    isChannelSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());
                }

                if (isChannelSearchable)
                {
                    filterArticles = filterArticles.Where(c => c.ChannelId == ChannelFilter.ToInt());
                }

                var isContentSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                if (isContentSearchable)
                {
                    filterArticles = filterArticles.Where(c => StringUtil.RemoveHtml(c.Content.ToLower()).Contains(ContentFilter));
                  //  filterArticles = filterArticles.Where(c => c.Content.ToLower().Contains(ContentFilter));
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterArticles = allArticles;
            }
            else
            {
                filterArticles = allArticles;
            }
            #endregion

            #region 排序
            var isTitleSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);



            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Article, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isTitleSortable ? c.Title :
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterArticles = filterArticles.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterArticles = filterArticles.OrderByDescending(orderingFunction);
            }

            #endregion

            var displayedArticles = filterArticles.Skip(request.start).Take(request.length);
            var result = from c in displayedArticles
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Title
                                            , c.Channel.Name                                            
                                            , StringUtil.CutString(StringUtil.RemoveHtml(c.Content), 100)
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allArticles.Count(),//alltransactions.Count(),
                recordsFiltered = filterArticles.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Cms/Article/

        public ActionResult Index(ArticleRequest request)
        {
            var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
            this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name");

           // var result = this.CmsService.GetArticleList(request);
           // return View(result);
            return View();
        }

        //
        // GET: /Cms/Article/Create

        public ActionResult Create()
        {
            var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
            this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name");
            this.ViewBag.Tags = this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits });

            var model = new Article() { IsActive = true };
            return PartialView("Edit", model);
        }

        //
        // POST: /Cms/Article/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Article() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };   
            this.TryUpdateModel<Article>(model);

            try
            {
                if (ModelState.IsValid)
                {                   

                    this.CmsService.SaveArticle(model);
                              
                }
                string url = Url.Action("Index", "Article");
                return Json(new { success = true, url = url });    

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
                this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name");
                this.ViewBag.Tags = this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits });

                return PartialView("Edit", model);
            }
        }

        //
        // GET: /Cms/Article/Edit/5

        public ActionResult Edit(int id)
        {
            var model = this.CmsService.GetArticle(id);

            var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
            this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name",model.ChannelId);
            this.ViewBag.Tags = this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits });

            return View(model);
        }

        //
        // POST: /Cms/Article/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {

            var model = this.CmsService.GetArticle(id);
            this.TryUpdateModel<Article>(model);

            try
            {
                if (ModelState.IsValid)
                {

                    this.CmsService.SaveArticle(model);
                }
                string url = Url.Action("Index", "Article");
                return Json(new { success = true, url = url }); 
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
                this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name");
                this.ViewBag.Tags = this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits });
            }
            return PartialView("Edit", model);
        }

        // POST: /Cms/Article/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.CmsService.DeleteArticle(ids);
            return RedirectToAction("Index");
        }
    }

}