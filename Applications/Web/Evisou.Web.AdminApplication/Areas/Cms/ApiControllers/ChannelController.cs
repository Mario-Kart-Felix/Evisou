using Evisou.Account.Contract;
using Evisou.Cms.Contract;
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
    public class ChannelController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] ChannelInquiryDTO channelInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ChannelApiModels channelWebApi = new ChannelApiModels();
            try
            {
                var all = this.CmsService.GetChannelList(null);
                IEnumerable<Channel> filter = all;
                if (!string.IsNullOrEmpty(channelInquiryDTO.Name))
                    filter = filter.Where(c => c.Name.Contains(channelInquiryDTO.Name));
                if (!string.IsNullOrEmpty(channelInquiryDTO.CoverPicture))
                    filter = filter.Where(c => c.CoverPicture.Contains(channelInquiryDTO.CoverPicture));

                int start = (channelInquiryDTO.CurrentPageNumber - 1) * channelInquiryDTO.PageSize;
                string sortDirection = channelInquiryDTO.SortDirection;
                string sortExpression = channelInquiryDTO.SortExpression;
                if (channelInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(channelInquiryDTO.PageSize);

                List<ChannelDTO> channelList = new List<ChannelDTO>();

                filter.ToList().ForEach(c =>
                {
                    channelList.Add(new ChannelDTO
                    {
                        ID = c.ID,
                        Name = c.Name,
                        CoverPicture = c.CoverPicture,
                        Hits = c.Hits,
                        Desc = c.Desc
                    });
                });

                 Func<ChannelDTO, string> orderingFunction = (
                         c => sortExpression.Contains("Name") ? c.Name :
                         sortExpression.Contains("CoverPicture") ? c.CoverPicture :
                         ""
                     );
                IEnumerable<ChannelDTO> Result = new List<ChannelDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = channelList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = channelList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = channelList;
                        break;

                };
                channelWebApi.Channels = Result;
                channelWebApi.TotalRecords = all.Count();
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            if (transaction.ReturnStatus == false)
            {
                channelWebApi.ReturnMessage = transaction.ReturnMessage;
                channelWebApi.ReturnStatus = transaction.ReturnStatus;
                channelWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, channelWebApi);
                return badResponse;
            }



            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int ChannelID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ChannelApiModels channelWebApi = new ChannelApiModels();
            try
            {
                var model = this.CmsService.GetChannel(ChannelID);
                channelWebApi.Channel = new ChannelDTO
                {
                    IsActive = model.IsActive,
                    CoverPicture = model.CoverPicture,
                    CreateTime = model.CreateTime,
                    Desc = model.Desc,
                    Hits = model.Hits,
                    Name = model.Name

                };
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            if (transaction.ReturnStatus == false)
            {
                channelWebApi.ReturnMessage = transaction.ReturnMessage;
                channelWebApi.ReturnStatus = transaction.ReturnStatus;
                channelWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, channelWebApi);
                return badResponse;
            }
            channelWebApi.ReturnMessage = transaction.ReturnMessage;
            channelWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, channelWebApi);
            
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ChannelApiModels channelWebApi = new ChannelApiModels();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        [HttpPost]
        public HttpResponseMessage Post([FromBody]ChannelDTO ChannelDTO)
        {

            Channel model = new Channel();
            model.Name = ChannelDTO.Name;
            model.IsActive = ChannelDTO.IsActive;
            model.Desc = ChannelDTO.Desc;
            model.Hits = ChannelDTO.Hits;
            model.CoverPicture = ChannelDTO.CoverPicture;
            
            return SaveChannel(model, "添加成功");
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]ChannelDTO ChannelDTO)
        {
            Channel model = this.CmsService.GetChannel(ChannelDTO.ID);
            model.Name = ChannelDTO.Name;
            model.IsActive = ChannelDTO.IsActive;
            model.Desc = ChannelDTO.Desc;
            model.Hits = ChannelDTO.Hits;
            model.CoverPicture = ChannelDTO.CoverPicture;
            return SaveChannel(model, "修改成功");
        }

        private HttpResponseMessage SaveChannel(Channel model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ChannelApiModels channelWebApi = new ChannelApiModels();
            try
            {
                this.CmsService.SaveChannel(model);
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
                channelWebApi.ReturnMessage = transaction.ReturnMessage;
                channelWebApi.ReturnStatus = transaction.ReturnStatus;
                channelWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, channelWebApi);
                return badResponse;
            }

            channelWebApi.IsAuthenicated = true;
            channelWebApi.ReturnStatus = transaction.ReturnStatus;
            channelWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, channelWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody] List<ChannelDTO> channels)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ChannelApiModels channelWebApi = new ChannelApiModels();
            try
            {
                channels.ForEach(c =>
                {
                    Channel model = this.CmsService.GetChannel(c.ID);
                    model.Name = c.Name;
                    model.IsActive = c.IsActive;
                    model.Desc = c.Desc;
                    model.Hits = c.Hits;
                    model.CoverPicture = c.CoverPicture;
                    this.CmsService.SaveChannel(model);
                });

                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            if (transaction.ReturnStatus == false)
            {
                channelWebApi.ReturnMessage = transaction.ReturnMessage;
                channelWebApi.ReturnStatus = transaction.ReturnStatus;
                channelWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, channelWebApi);
                return badResponse;
            }
            string returnMessage = "批量修改成功";
            channelWebApi.IsAuthenicated = true;
            channelWebApi.ReturnStatus = transaction.ReturnStatus;
            channelWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, channelWebApi);
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> ChannelID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ChannelApiModels channelWebApi = new ChannelApiModels();
            try
            {
                this.CmsService.DeleteChannel(ChannelID);
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }
            if (transaction.ReturnStatus == false)
            {
                channelWebApi.ReturnMessage = transaction.ReturnMessage;
                channelWebApi.ReturnStatus = transaction.ReturnStatus;
                channelWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, channelWebApi);
                return badResponse;
            }
            string returnMessage = "批量删除成功";
            channelWebApi.IsAuthenicated = true;
            channelWebApi.ReturnStatus = transaction.ReturnStatus;
            channelWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, channelWebApi);
        }
    }
}
