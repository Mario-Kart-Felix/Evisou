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
    [WebApiPermission(EnumBusinessPermission.ImsManage_Purchase)]
    public class PurchaseController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] PurchaseInquiryDTO purchaseInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PurchaseApiModels purchaseWebApi = new PurchaseApiModels();
            try
            {
                var all = this.ImsService.GetPurchaseList(null);
                IEnumerable<Purchase> filter = all;
                if (!string.IsNullOrEmpty(purchaseInquiryDTO.PurchaseTransactionNumber))
                    filter = filter.Where(c => c.PurchaseTransactionID.Contains(purchaseInquiryDTO.PurchaseTransactionNumber));
               

                int start = (purchaseInquiryDTO.CurrentPageNumber - 1) * purchaseInquiryDTO.PageSize;
                string sortDirection = purchaseInquiryDTO.SortDirection;
                string sortExpression = purchaseInquiryDTO.SortExpression;
                if (purchaseInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(purchaseInquiryDTO.PageSize);
                List<PurchaseDTO> purchaseList = new List<PurchaseDTO>();
                filter.ToList().ForEach(c =>
                {
                    purchaseList.Add(new PurchaseDTO
                    {
                        PurchaseDate = c.PurchaseDate,
                        PurchaseTransactionNumber = c.PurchaseTransactionID,
                        SupplierID = c.SupplierID,
                        UserId = c.UserId
                    });
                });

                Func<PurchaseDTO, string> orderingFunction = (
                        c => sortExpression.Contains("PurchaseTransactionNumber") ? c.PurchaseTransactionNumber :
                        //sortExpression.Contains("AgentName") ? c.AgentName :
                        ""
                    );
                IEnumerable<PurchaseDTO> Result = new List<PurchaseDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = purchaseList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = purchaseList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = purchaseList;
                        break;

                };
                purchaseWebApi.Purchases = Result;
                purchaseWebApi.TotalRecords = all.Count();
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
                purchaseWebApi.ReturnMessage = transaction.ReturnMessage;
                purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
                purchaseWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, purchaseWebApi);
                return badResponse;
            }

            purchaseWebApi.IsAuthenicated = true;
            purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, purchaseWebApi);
            
        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int PurchaseID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PurchaseApiModels purchaseWebApi = new PurchaseApiModels();
            try
            {
                var model = this.ImsService.GetPurchase(PurchaseID);
                purchaseWebApi.Purchase = new PurchaseDTO
                {

                    PurchaseDate = model.PurchaseDate,
                    PurchaseTransactionNumber = model.PurchaseTransactionID,
                    SupplierID = model.SupplierID,

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
                purchaseWebApi.ReturnMessage = transaction.ReturnMessage;
                purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
                purchaseWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, purchaseWebApi);
                return badResponse;
            }
            string returnMessage = "";
            purchaseWebApi.IsAuthenicated = true;
            purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
            purchaseWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, purchaseWebApi);
            
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]PurchaseDTO PurchaseDTO)
        {
            Purchase model = new Purchase
            {
                PurchaseDate = PurchaseDTO.PurchaseDate,

            };
            return SavePurchase(model, "添加成功");
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]PurchaseDTO PurchaseDTO)
        {
            Purchase model = this.ImsService.GetPurchase(PurchaseDTO.ID);
            model.PurchaseDate = PurchaseDTO.PurchaseDate;
            model.PurchaseTransactionID = PurchaseDTO.PurchaseTransactionNumber;
            return SavePurchase(model, "修改成功");
            
        }
        private HttpResponseMessage SavePurchase(Purchase model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PurchaseApiModels purchaseWebApi = new PurchaseApiModels();
            try
            {
                this.ImsService.SavePurchase(model);
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
                purchaseWebApi.ReturnMessage = transaction.ReturnMessage;
                purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
                purchaseWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, purchaseWebApi);
                return badResponse;
            }

            purchaseWebApi.IsAuthenicated = true;
            purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
            purchaseWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, purchaseWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody]List<PurchaseDTO> purchases)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PurchaseApiModels purchaseWebApi = new PurchaseApiModels();
            try
            {
                purchases.ForEach(c =>
                {
                    Purchase model = this.ImsService.GetPurchase(c.ID);
                    model.PurchaseDate = c.PurchaseDate;
                    model.PurchaseTransactionID = c.PurchaseTransactionNumber;
                    this.ImsService.SavePurchase(model);
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
                purchaseWebApi.ReturnMessage = transaction.ReturnMessage;
                purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
                purchaseWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, purchaseWebApi);
                return badResponse;
            }
            string returnMessage = "批量修改成功";
            purchaseWebApi.IsAuthenicated = true;
            purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
            purchaseWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, purchaseWebApi);

        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> PurchaseID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            PurchaseApiModels purchaseWebApi = new PurchaseApiModels();
            try
            {
                this.ImsService.DeletePurchase(PurchaseID);
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
                purchaseWebApi.ReturnMessage = transaction.ReturnMessage;
                purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
                purchaseWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, purchaseWebApi);
                return badResponse;
            }
            string returnMessage = "批量删除成功";
            purchaseWebApi.IsAuthenicated = true;
            purchaseWebApi.ReturnStatus = transaction.ReturnStatus;
            purchaseWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, purchaseWebApi);
        }
    }
}
