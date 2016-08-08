using Evisou.Account.Contract;
using Evisou.Cms.Contract;
using Evisou.Framework.Utility;
using Evisou.Web.AdminApplication.Areas.Cms.WebApiModels;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evisou.Web.AdminApplication.Areas.Cms.ApiControllers
{
    [WebApiPermission(EnumBusinessPermission.CmsManage_Article)]
    public class ArticleController : AdminApiControllerBase
    {
        
        [HttpGet]
        public HttpResponseMessage Get([FromUri] ArticleInquiryDTO articleInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ArticleApiModels articleWebApi = new ArticleApiModels();
            try
            {
                var all = this.CmsService.GetArticleList(null);
                IEnumerable<Article> filter = all;
                if (!string.IsNullOrEmpty(articleInquiryDTO.Title))
                    filter = filter.Where(c => c.Title.Contains(articleInquiryDTO.Title));
                if (!string.IsNullOrEmpty(articleInquiryDTO.Content))
                    filter = filter.Where(c => c.Content.Contains(articleInquiryDTO.Content));


                int start = (articleInquiryDTO.CurrentPageNumber - 1) * articleInquiryDTO.PageSize;
                string sortDirection = articleInquiryDTO.SortDirection;
                string sortExpression = articleInquiryDTO.SortExpression;
                if (articleInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(articleInquiryDTO.PageSize);

                List<ArticleDTO> articleList = new List<ArticleDTO>();
                filter.ToList().ForEach(c =>
                {
                    articleList.Add(new ArticleDTO
                    {
                        ID = c.ID,
                        Title = c.Title,
                        Channel = new ChannelDTO
                        {
                            Name = c.Channel.Name
                        },
                        Content = StringUtil.CutString(StringUtil.RemoveHtml(c.Content), 100),

                    });
                });

                Func<ArticleDTO, string> orderingFunction = (
                         c => sortExpression.Contains("Title") ? c.Title :
                         sortExpression.Contains("Content") ? c.Content :
                         ""
                     );
                IEnumerable<ArticleDTO> Result = new List<ArticleDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = articleList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = articleList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = articleList;
                        break;

                };
                var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
                List<ChannelDTO> Channels = new List<ChannelDTO>();
                channelList.ToList().ForEach(c =>
                {
                    Channels.Add(new ChannelDTO
                    {
                        IsActive = c.IsActive,
                        Name = c.Name,
                        ID=c.ID
                    });
                });
                List<TagDTO> Tags = new List<TagDTO>();
                this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits }).ToList().ForEach(c => {
                    Tags.Add(new TagDTO
                    {
                        Hits = c.Hits,
                        Name = c.Name
                    });
                });
                articleWebApi.Article.Tags = Tags;
                articleWebApi.Article.Channels = Channels;
                articleWebApi.Articles = Result;
                articleWebApi.TotalRecords = all.Count();
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                articleWebApi.ReturnMessage = transaction.ReturnMessage;
                articleWebApi.ReturnStatus = transaction.ReturnStatus;
                articleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, articleWebApi);
                return badResponse;
            }
            articleWebApi.ReturnMessage = transaction.ReturnMessage;
            articleWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, articleWebApi);
        }

      
        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int ArticleID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ArticleApiModels articleWebApi = new ArticleApiModels();
            try
            {
               List<TagDTO> Tags = new List<TagDTO>();
               //this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits }).ToList().ForEach(c => {
               //     Tags.Add(new TagDTO
               //     {
               //         Hits = c.Hits,
               //         Name = c.Name
               //     });
               // });
                var model = this.CmsService.GetArticle(ArticleID);
                model.Tags.ToList().ForEach(c =>
                {
                    Tags.Add(new TagDTO
                    {
                        Hits = c.Hits,
                        Name = c.Name
                    });
                });


                articleWebApi.Article = new ArticleDTO
                {
                    Title = model.Title,
                    Content = model.Content,
                    IsActive=model.IsActive,
                    CoverPicture=model.CoverPicture,
                    SelectedChannelId=model.ChannelId,
                   Tags= Tags

                };
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                articleWebApi.ReturnMessage = transaction.ReturnMessage;
                articleWebApi.ReturnStatus = transaction.ReturnStatus;
                articleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, articleWebApi);
                return badResponse;
            }

            articleWebApi.ReturnMessage = transaction.ReturnMessage;
            articleWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, articleWebApi);
        }

        [HttpGet]
        public HttpResponseMessage Export([FromUri]  List<int> IDs, [FromUri] string DataType)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }

       
        [HttpPost]
        public HttpResponseMessage Post([FromBody]ArticleDTO articleDTO)
        {
            Article model = new Article() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            model.Title = articleDTO.Title;
            model.ChannelId = articleDTO.Channel.ID;
            model.IsActive = articleDTO.IsActive;
            model.TagString = articleDTO.TagString;
            return SaveArticle(model, "添加成功");
        }

       
        [HttpPut]
        public HttpResponseMessage Put([FromBody]ArticleDTO articleDTO)
        {
            Article model = this.CmsService.GetArticle(articleDTO.ID);
            model.Title = articleDTO.Title;
            model.ChannelId = articleDTO.Channel.ID;
            model.IsActive = articleDTO.IsActive;
            model.TagString = articleDTO.TagString;
            return SaveArticle(model, "修改成功");
        }

        private HttpResponseMessage SaveArticle(Article model,string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ArticleApiModels articleWebApi = new ArticleApiModels();
            try
            {
                this.CmsService.SaveArticle(model);
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                articleWebApi.ReturnMessage = transaction.ReturnMessage;
                articleWebApi.ReturnStatus = transaction.ReturnStatus;
                articleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, articleWebApi);
                return badResponse;
            }

            articleWebApi.IsAuthenicated = true;
            articleWebApi.ReturnStatus = transaction.ReturnStatus;
            articleWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, articleWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody] List<ArticleDTO> articles)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ArticleApiModels articleWebApi = new ArticleApiModels();
            
            try
            {
                articles.ForEach(c =>
                {
                    Article model = this.CmsService.GetArticle(c.ID);
                    model.Title = c.Title;
                    model.ChannelId = c.Channel.ID;
                    model.IsActive = c.IsActive;
                    model.TagString = c.TagString;
                });
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                articleWebApi.ReturnMessage = transaction.ReturnMessage;
                articleWebApi.ReturnStatus = transaction.ReturnStatus;
                articleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, articleWebApi);
                return badResponse;
            }
            string returnMessage = "批量修改成功";
            articleWebApi.IsAuthenicated = true;
            articleWebApi.ReturnStatus = transaction.ReturnStatus;
            articleWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, articleWebApi);
        }

       
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> ArticleIDs)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ArticleApiModels articleWebApi = new ArticleApiModels();

            try
            {
                this.CmsService.DeleteArticle(ArticleIDs);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            if (transaction.ReturnStatus == false)
            {
                articleWebApi.ReturnMessage = transaction.ReturnMessage;
                articleWebApi.ReturnStatus = transaction.ReturnStatus;
                articleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, articleWebApi);
                return badResponse;
            }

            string returnMessage = "批量删除成功";
            articleWebApi.IsAuthenicated = true;
            articleWebApi.ReturnStatus = transaction.ReturnStatus;
            articleWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, articleWebApi);
        }

        
    }
}