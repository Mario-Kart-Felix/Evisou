using Evisou.Account.Contract;
using Evisou.Ims.Contract.Model;
using Evisou.Web.AdminApplication.Areas.Ims.WebApiModels;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evisou.Web.AdminApplication.Areas.Ims.ApiControllers
{
    [WebApiPermission(EnumBusinessPermission.ImsManage_Agent)]
    public class AgentController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] AgentInquiryDTO agentInquiryDTO)
        {

            TransactionalInformation transaction = new TransactionalInformation();
            AgentApiModels agentWebApi = new AgentApiModels();
            try
            {
                var all = this.ImsService.GetAgentList(null);
                IEnumerable<Agent> filter = all;
                if (!string.IsNullOrEmpty(agentInquiryDTO.AgentName))
                    filter = filter.Where(c => c.AgentName.Contains(agentInquiryDTO.AgentName));
                if (!string.IsNullOrEmpty(agentInquiryDTO.AgentCode))
                    filter = filter.Where(c => c.AgentCode.Contains(agentInquiryDTO.AgentCode));

                int start = (agentInquiryDTO.CurrentPageNumber - 1) * agentInquiryDTO.PageSize;
                string sortDirection = agentInquiryDTO.SortDirection;
                string sortExpression = agentInquiryDTO.SortExpression;
                if (agentInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(agentInquiryDTO.PageSize);

                List<AgentDTO> agentList = new List<AgentDTO>();

                filter.ToList().ForEach(c =>
                {
                    agentList.Add(new AgentDTO
                    {
                        ID = c.ID,
                        Address = c.Address,
                        AgentCode = c.AgentCode,
                        AgentName = c.AgentName,
                        Consignee = c.Consignee,
                        ConsigneeTel = c.ConsigneeTel,
                        CS = c.CS,
                        Email = c.Email,
                        CSQQ = c.CSQQ,
                        WebURL = c.WebURL,
                        SaleManager = c.SaleManager,
                        SaleMangerMobile = c.SaleMangerMobile,
                        UserId = c.UserId
                    });
                });

                Func<AgentDTO, string> orderingFunction = (
                        c => sortExpression.Contains("AgentCode") ? c.AgentCode :
                        sortExpression.Contains("AgentName") ? c.AgentName :
                        ""
                    );
                IEnumerable<AgentDTO> Result = new List<AgentDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = agentList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = agentList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = agentList;
                        break;

                };
                agentWebApi.Agents = Result;
                agentWebApi.TotalRecords = all.Count();

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
                agentWebApi.ReturnMessage = transaction.ReturnMessage;
                agentWebApi.ReturnStatus = transaction.ReturnStatus;
                agentWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, agentWebApi);
                return badResponse;
            }
            agentWebApi.ReturnMessage = transaction.ReturnMessage;
            agentWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, agentWebApi);
        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int AgentID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AgentApiModels agentWebApi = new AgentApiModels();
            try
            {
                var model = this.ImsService.GetAgent(AgentID);
                agentWebApi.Agent = new AgentDTO
                {
                    ID = model.ID,
                    Address = model.Address,
                    AgentCode = model.AgentCode,
                    AgentName = model.AgentName,
                    Consignee = model.Consignee,
                    ConsigneeTel = model.ConsigneeTel,
                    CS = model.CS,
                    Email = model.Email,
                    CSQQ = model.CSQQ,
                    WebURL = model.WebURL,
                    SaleManager = model.SaleManager,
                    SaleMangerMobile = model.SaleMangerMobile,
                    UserId = model.UserId
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
                agentWebApi.ReturnMessage = transaction.ReturnMessage;
                agentWebApi.ReturnStatus = transaction.ReturnStatus;
                agentWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, agentWebApi);
                return badResponse;
            }
            agentWebApi.ReturnMessage = transaction.ReturnMessage;
            agentWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, agentWebApi);
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]AgentDTO AgentDTO)
        {
            Agent model = new Agent
            {
                ID = AgentDTO.ID,
                Address = AgentDTO.Address,
                AgentCode = AgentDTO.AgentCode,
                AgentName = AgentDTO.AgentName,
                Consignee = AgentDTO.Consignee,
                ConsigneeTel = AgentDTO.ConsigneeTel,
                CS = AgentDTO.CS,
                Email = AgentDTO.Email,
                CSQQ = AgentDTO.CSQQ,
                WebURL = AgentDTO.WebURL,
                SaleManager = AgentDTO.SaleManager,
                SaleMangerMobile = AgentDTO.SaleMangerMobile,
                UserId = AgentDTO.UserId
            };

            return SaveAgent(model, "添加成功");
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]AgentDTO AgentDTO)
        {
            Agent model = this.ImsService.GetAgent(AgentDTO.ID);
            model.Address = AgentDTO.Address;
            model.AgentCode = AgentDTO.AgentCode;
            model.AgentName = AgentDTO.AgentName;
            model.Consignee = AgentDTO.Consignee;
            model.ConsigneeTel = AgentDTO.ConsigneeTel;
            model.CS = AgentDTO.CS;
            model.Email = AgentDTO.Email;
            model.CSQQ = AgentDTO.CSQQ;
            model.WebURL = AgentDTO.WebURL;
            model.SaleManager = AgentDTO.SaleManager;
            model.SaleMangerMobile = AgentDTO.SaleMangerMobile;
            model.UserId = AgentDTO.UserId;
            return SaveAgent(model, "修改成功");
        }
        private HttpResponseMessage SaveAgent(Agent model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AgentApiModels agentWebApi = new AgentApiModels();
            try
            {
                this.ImsService.SaveAgent(model);
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
                agentWebApi.ReturnMessage = transaction.ReturnMessage;
                agentWebApi.ReturnStatus = transaction.ReturnStatus;
                agentWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, agentWebApi);
                return badResponse;
            }

            agentWebApi.IsAuthenicated = true;
            agentWebApi.ReturnStatus = transaction.ReturnStatus;
            agentWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, agentWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody]List<AgentDTO> agents)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AgentApiModels agentWebApi = new AgentApiModels();
            try
            {
                agents.ForEach(c =>
                {

                    Agent model = this.ImsService.GetAgent(c.ID);
                    model.Address = c.Address;
                    model.AgentCode = c.AgentCode;
                    model.AgentName = c.AgentName;
                    model.Consignee = c.Consignee;
                    model.ConsigneeTel = c.ConsigneeTel;
                    model.CS = c.CS;
                    model.Email = c.Email;
                    model.CSQQ = c.CSQQ;
                    model.WebURL = c.WebURL;
                    model.SaleManager = c.SaleManager;
                    model.SaleMangerMobile = c.SaleMangerMobile;
                    model.UserId = c.UserId;
                    this.ImsService.SaveAgent(model);
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
                agentWebApi.ReturnMessage = transaction.ReturnMessage;
                agentWebApi.ReturnStatus = transaction.ReturnStatus;
                agentWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, agentWebApi);
                return badResponse;
            }
            string returnMessage = "批量修改成功";
            agentWebApi.IsAuthenicated = true;
            agentWebApi.ReturnStatus = transaction.ReturnStatus;
            agentWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, agentWebApi);
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> AgentID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AgentApiModels agentWebApi = new AgentApiModels();
            try
            {
                this.ImsService.DeleteAgent(AgentID);
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
                agentWebApi.ReturnMessage = transaction.ReturnMessage;
                agentWebApi.ReturnStatus = transaction.ReturnStatus;
                agentWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, agentWebApi);
                return badResponse;
            }
            string returnMessage = "批量删除成功";
            agentWebApi.IsAuthenicated = true;
            agentWebApi.ReturnStatus = transaction.ReturnStatus;
            agentWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, agentWebApi);
        }
    }
}
