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
    [Permission(EnumBusinessPermission.ImsManage_Association)]
    public class AssociationController : AdminApiControllerBase
    {

        [HttpGet]
        public HttpResponseMessage Get([FromUri] AssociationInquiryDTO associationInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AssociationApiModels associationWebApi = new AssociationApiModels();
            try
            {
                var all = this.ImsService.GetAssociationList(null);
                IEnumerable<Association> filter = all;
                if (!string.IsNullOrEmpty(associationInquiryDTO.ItemNumber))
                    filter = filter.Where(c => c.ItemNumber.Contains(associationInquiryDTO.ItemNumber));
               

                int start = (associationInquiryDTO.CurrentPageNumber - 1) * associationInquiryDTO.PageSize;
                string sortDirection = associationInquiryDTO.SortDirection;
                string sortExpression = associationInquiryDTO.SortExpression;
                if (associationInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(associationInquiryDTO.PageSize);
                List<AssociationDTO> associationList = new List<AssociationDTO>();
                filter.ToList().ForEach(c =>
                {
                    associationList.Add(new AssociationDTO
                    {
                        PaypalApiID = c.PaypalApiID,
                        ProductID = c.ProductID,
                        ItemTitle = c.ItemTitle,
                        ItemNumber = c.ItemNumber,
                        StorePlace = c.StorePlace,
                        SellingPlace = c.SellingPlace,
                        UserId = c.UserId
                    });
                });

                Func<AssociationDTO, string> orderingFunction = (
                        c => sortExpression.Contains("ItemNumber") ? c.ItemNumber :
                        sortExpression.Contains("StorePlace") ? c.StorePlace :
                        ""
                    );
                IEnumerable<AssociationDTO> Result = new List<AssociationDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = associationList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = associationList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = associationList;
                        break;

                };
                associationWebApi.Associations = Result;
                associationWebApi.TotalRecords = all.Count();
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
                associationWebApi.ReturnMessage = transaction.ReturnMessage;
                associationWebApi.ReturnStatus = transaction.ReturnStatus;
                associationWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, associationWebApi);
                return badResponse;
            }
            associationWebApi.ReturnMessage = transaction.ReturnMessage;
            associationWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, associationWebApi);
        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int AssociationID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AssociationApiModels associationWebApi = new AssociationApiModels();
            try
            {
                var model = this.ImsService.GetAssociation(AssociationID);
                associationWebApi.Association = new AssociationDTO
                {
                    ItemNumber = model.ItemNumber,
                    ItemTitle = model.ItemTitle,
                    PaypalApiID = model.PaypalApiID,
                    ProductID = model.PaypalApiID,
                    SellingPlace = model.SellingPlace,
                    StorePlace = model.StorePlace,

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
                associationWebApi.ReturnMessage = transaction.ReturnMessage;
                associationWebApi.ReturnStatus = transaction.ReturnStatus;
                associationWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, associationWebApi);
                return badResponse;
            }

            associationWebApi.ReturnMessage = transaction.ReturnMessage;
            associationWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, associationWebApi);
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] AssociationDTO AssociationDTO)
        {
            Association model = new Association
            {
                PaypalApiID = AssociationDTO.PaypalApiID,
                ProductID = AssociationDTO.ProductID,
                ItemTitle = AssociationDTO.ItemTitle,
                ItemNumber = AssociationDTO.ItemNumber,
                StorePlace = AssociationDTO.StorePlace,
                SellingPlace = AssociationDTO.SellingPlace,
                UserId = AssociationDTO.UserId
            };

            return SaveAssociation(model, "添加成功");
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody] AssociationDTO AssociationDTO)
        {
            Association model = this.ImsService.GetAssociation(AssociationDTO.ID);
            model.PaypalApiID = AssociationDTO.PaypalApiID;
            model.ProductID = AssociationDTO.ProductID;
            model.ItemTitle = AssociationDTO.ItemTitle;
            model.ItemNumber = AssociationDTO.ItemNumber;
            model.StorePlace = AssociationDTO.StorePlace;
            model.SellingPlace = AssociationDTO.SellingPlace;
            model.UserId = AssociationDTO.UserId;
            return SaveAssociation(model, "修改成功");
        }

        private HttpResponseMessage SaveAssociation(Association model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AssociationApiModels associationWebApi = new AssociationApiModels();
            try
            {
                this.ImsService.SaveAssociation(model);
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
                associationWebApi.ReturnMessage = transaction.ReturnMessage;
                associationWebApi.ReturnStatus = transaction.ReturnStatus;
                associationWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, associationWebApi);
                return badResponse;
            }

            associationWebApi.IsAuthenicated = true;
            associationWebApi.ReturnStatus = transaction.ReturnStatus;
            associationWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, associationWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody]List<AssociationDTO> associations)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AssociationApiModels associationWebApi = new AssociationApiModels();

            try
            {
                associations.ForEach(c =>
                {
                    Association model = this.ImsService.GetAssociation(c.ID);
                    model.PaypalApiID = c.PaypalApiID;
                    model.ProductID = c.ProductID;
                    model.ItemTitle = c.ItemTitle;
                    model.ItemNumber = c.ItemNumber;
                    model.StorePlace = c.StorePlace;
                    model.SellingPlace = c.SellingPlace;
                    model.UserId = c.UserId;
                    this.ImsService.SaveAssociation(model);
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
                associationWebApi.ReturnMessage = transaction.ReturnMessage;
                associationWebApi.ReturnStatus = transaction.ReturnStatus;
                associationWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, associationWebApi);
                return badResponse;
            }

            associationWebApi.ReturnMessage = transaction.ReturnMessage;
            associationWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, associationWebApi);
            
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> AssociationID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            AssociationApiModels associationWebApi = new AssociationApiModels();
            try
            {
                this.ImsService.DeleteAssociation(AssociationID);
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
                associationWebApi.ReturnMessage = transaction.ReturnMessage;
                associationWebApi.ReturnStatus = transaction.ReturnStatus;
                associationWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, associationWebApi);
                return badResponse;
            }
            associationWebApi.ReturnMessage = transaction.ReturnMessage;
            associationWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, associationWebApi);
        }
    }
}
