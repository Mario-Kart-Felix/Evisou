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
    [Permission(EnumBusinessPermission.ImsManage_PaypalApi)]
    public class PaypalApiController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] PaypalApiInquiryDTO paypalApiInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PaypalApiModels paypalWebApi = new PaypalApiModels();
            try
            {
                var all = this.ImsService.GetPaypalApiList(null);
                IEnumerable<PaypalApi> filter = all;
                if (!string.IsNullOrEmpty(paypalApiInquiryDTO.PPAccount))
                    filter = filter.Where(c => c.PPAccount.Contains(paypalApiInquiryDTO.PPAccount));
                if (!string.IsNullOrEmpty(paypalApiInquiryDTO.ApiUserName))
                    filter = filter.Where(c => c.ApiUserName.Contains(paypalApiInquiryDTO.ApiUserName));

                int start = (paypalApiInquiryDTO.CurrentPageNumber - 1) * paypalApiInquiryDTO.PageSize;
                string sortDirection = paypalApiInquiryDTO.SortDirection;
                string sortExpression = paypalApiInquiryDTO.SortExpression;
                if (paypalApiInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(paypalApiInquiryDTO.PageSize);

                List<PaypalApiDTO> paypalApiList = new List<PaypalApiDTO>();

                filter.ToList().ForEach(c =>
                {
                    paypalApiList.Add(new PaypalApiDTO
                    {
                        ID = c.ID,
                        UserId = c.UserId,
                        ApiUserName = c.ApiUserName,
                        IsActive = c.IsActive,
                        PPAccount = c.PPAccount,
                        Signature = c.Signature

                    });
                });

                Func<PaypalApiDTO, string> orderingFunction = (
                        c => sortExpression.Contains("PPAccount") ? c.PPAccount :
                        sortExpression.Contains("ApiUserName") ? c.ApiUserName :
                        ""
                    );
                IEnumerable<PaypalApiDTO> Result = new List<PaypalApiDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = paypalApiList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = paypalApiList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = paypalApiList;
                        break;

                };
                paypalWebApi.PaypalApis= Result;
                paypalWebApi.TotalRecords = all.Count();

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
                paypalWebApi.ReturnMessage = transaction.ReturnMessage;
                paypalWebApi.ReturnStatus = transaction.ReturnStatus;
                paypalWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paypalWebApi);
                return badResponse;
            }
            paypalWebApi.ReturnMessage = transaction.ReturnMessage;
            paypalWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, paypalWebApi);

        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int PaypalApiID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PaypalApiModels paypalWebApi = new PaypalApiModels();
            try
            {
                var model = this.ImsService.GetPaypalApi(PaypalApiID);
                paypalWebApi.PaypalApi = new PaypalApiDTO
                {
                    ID = model.ID,
                    UserId = model.UserId,
                    ApiUserName = model.ApiUserName,
                    IsActive = model.IsActive,
                    PPAccount = model.PPAccount,
                    Signature = model.Signature
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
                paypalWebApi.ReturnMessage = transaction.ReturnMessage;
                paypalWebApi.ReturnStatus = transaction.ReturnStatus;
                paypalWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paypalWebApi);
                return badResponse;
            }

            paypalWebApi.ReturnMessage = transaction.ReturnMessage;
            paypalWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, paypalWebApi);
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]PaypalApiDTO PaypalApiDTO)
        {

            PaypalApi model = new PaypalApi
            {
                ID = PaypalApiDTO.ID,
                UserId = PaypalApiDTO.UserId,
                ApiUserName = PaypalApiDTO.ApiUserName,
                IsActive = PaypalApiDTO.IsActive,
                PPAccount = PaypalApiDTO.PPAccount,
                Signature = PaypalApiDTO.Signature
            };
            return SavePaypalApi(model, "添加成功");
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]PaypalApiDTO PaypalApiDTO)
        {
            PaypalApi model = this.ImsService.GetPaypalApi(PaypalApiDTO.ID);
            model.ID = PaypalApiDTO.ID;
            model.UserId = PaypalApiDTO.UserId;
            model.ApiUserName = PaypalApiDTO.ApiUserName;
            model.IsActive = PaypalApiDTO.IsActive;
            model.PPAccount = PaypalApiDTO.PPAccount;
            model.Signature = PaypalApiDTO.Signature;
            return SavePaypalApi(model, "修改成功");
        }
        private HttpResponseMessage SavePaypalApi(PaypalApi model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PaypalApiModels paypalApiWebApi = new PaypalApiModels();
            try
            {
                this.ImsService.SavePaypalApi(model);
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
                paypalApiWebApi.ReturnMessage = transaction.ReturnMessage;
                paypalApiWebApi.ReturnStatus = transaction.ReturnStatus;
                paypalApiWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paypalApiWebApi);
                return badResponse;
            }

            paypalApiWebApi.IsAuthenicated = true;
            paypalApiWebApi.ReturnStatus = transaction.ReturnStatus;
            paypalApiWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, paypalApiWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody]List<PaypalApiDTO> paypals)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PaypalApiModels paypalWebApi = new PaypalApiModels();
            try
            {
                paypals.ForEach(c => {
                    PaypalApi model = this.ImsService.GetPaypalApi(c.ID);
                    model.ID = c.ID;
                    model.UserId = c.UserId;
                    model.ApiUserName = c.ApiUserName;
                    model.IsActive = c.IsActive;
                    model.PPAccount = c.PPAccount;
                    model.Signature = c.Signature;
                    this.ImsService.SavePaypalApi(model);
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
                paypalWebApi.ReturnMessage = transaction.ReturnMessage;
                paypalWebApi.ReturnStatus = transaction.ReturnStatus;
                paypalWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paypalWebApi);
                return badResponse;
            }

            paypalWebApi.ReturnMessage = transaction.ReturnMessage;
            paypalWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, paypalWebApi);
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> PaypalApiID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PaypalApiModels paypalWebApi = new PaypalApiModels();
            try
            {
                this.ImsService.DeletePaypalApi(PaypalApiID);
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
                paypalWebApi.ReturnMessage = transaction.ReturnMessage;
                paypalWebApi.ReturnStatus = transaction.ReturnStatus;
                paypalWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, paypalWebApi);
                return badResponse;
            }
            paypalWebApi.ReturnMessage = transaction.ReturnMessage;
            paypalWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, paypalWebApi);
        }
    }
}
