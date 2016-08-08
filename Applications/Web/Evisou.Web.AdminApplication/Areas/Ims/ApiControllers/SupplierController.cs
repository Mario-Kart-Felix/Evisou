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
    [WebApiPermission(EnumBusinessPermission.ImsManage_Supplier)]
    public class SupplierController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] SupplierInquiryDTO supplierInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            SupplierApiModels supplierWebApi = new SupplierApiModels();
            try
            {
                var all = this.ImsService.GetSupplierList(null);
                IEnumerable<Supplier> filter = all;
                if (!string.IsNullOrEmpty(supplierInquiryDTO.Name))
                    filter = filter.Where(c => c.Name.Contains(supplierInquiryDTO.Name));


                int start = (supplierInquiryDTO.CurrentPageNumber - 1) * supplierInquiryDTO.PageSize;
                string sortDirection = supplierInquiryDTO.SortDirection;
                string sortExpression = supplierInquiryDTO.SortExpression;
                if (supplierInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(supplierInquiryDTO.PageSize);
                List<SupplierDTO> supplierList = new List<SupplierDTO>();
                filter.ToList().ForEach(c =>
                {
                    supplierList.Add(new SupplierDTO
                    {
                        Name = c.Name,
                        Address = c.Address,
                        Contact = c.Contact,
                        ContactPhone = c.ContactPhone,
                        CS = c.CS,
                        CSPhone = c.CSPhone,
                        Platform = c.Platform,
                        ID = c.ID,
                        URL = c.URL,
                        UserId = c.UserId
                    });
                });

                Func<SupplierDTO, string> orderingFunction = (
                        c => sortExpression.Contains("Name") ? c.Name :
                        //sortExpression.Contains("AgentName") ? c.AgentName :
                        ""
                    );
                IEnumerable<SupplierDTO> Result = new List<SupplierDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = supplierList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = supplierList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = supplierList;
                        break;

                };
                supplierWebApi.Suppliers = Result;
                supplierWebApi.TotalRecords = all.Count();
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
                supplierWebApi.ReturnMessage = transaction.ReturnMessage;
                supplierWebApi.ReturnStatus = transaction.ReturnStatus;
                supplierWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, supplierWebApi);
                return badResponse;
            }
            string returnMessage = "";
            supplierWebApi.IsAuthenicated = true;
            supplierWebApi.ReturnStatus = transaction.ReturnStatus;
            supplierWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, supplierWebApi);
            
        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int SupplierID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            SupplierApiModels supplierWebApi = new SupplierApiModels();
            try
            {
                Supplier model = this.ImsService.GetSupplier(SupplierID);
                supplierWebApi.Supplier = new SupplierDTO
                {
                    Name = model.Name,
                    Address = model.Address,
                    Contact = model.Contact,
                    ContactPhone = model.ContactPhone,
                    CS = model.CS,
                    CSPhone = model.CSPhone,
                    Platform = model.Platform,
                    ID = model.ID,
                    URL = model.URL,
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
                supplierWebApi.ReturnMessage = transaction.ReturnMessage;
                supplierWebApi.ReturnStatus = transaction.ReturnStatus;
                supplierWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, supplierWebApi);
                return badResponse;
            }
            string returnMessage = "";
            supplierWebApi.IsAuthenicated = true;
            supplierWebApi.ReturnStatus = transaction.ReturnStatus;
            supplierWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, supplierWebApi);

            
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]SupplierDTO SupplierDTO)
        {
            Supplier model = new Supplier
            {
                Name = SupplierDTO.Name,
                Address = SupplierDTO.Address,
                Contact = SupplierDTO.Contact,
                ContactPhone = SupplierDTO.ContactPhone,
                CS = SupplierDTO.CS,
                CSPhone = SupplierDTO.CSPhone,
                Platform = SupplierDTO.Platform,
                ID = SupplierDTO.ID,
                URL = SupplierDTO.URL,
                UserId = SupplierDTO.UserId
            };

            return SaveSupplier(model, "添加成功");
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]SupplierDTO SupplierDTO)
        {
            Supplier model = this.ImsService.GetSupplier(SupplierDTO.ID);
            model.Name = SupplierDTO.Name;
            model.Address = SupplierDTO.Address;
            model.Contact = SupplierDTO.Contact;
            model.ContactPhone = SupplierDTO.ContactPhone;
            model.CS = SupplierDTO.CS;
            model.CSPhone = SupplierDTO.CSPhone;
            model.Platform = SupplierDTO.Platform;
            model.ID = SupplierDTO.ID;
            model.URL = SupplierDTO.URL;
            model.UserId = SupplierDTO.UserId;
            return SaveSupplier(model, "修改成功");
           
        }
        private HttpResponseMessage SaveSupplier(Supplier model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            SupplierApiModels supplierWebApi = new SupplierApiModels();
            try
            {
                this.ImsService.SaveSupplier(model);
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
                supplierWebApi.ReturnMessage = transaction.ReturnMessage;
                supplierWebApi.ReturnStatus = transaction.ReturnStatus;
                supplierWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, supplierWebApi);
                return badResponse;
            }

            supplierWebApi.IsAuthenicated = true;
            supplierWebApi.ReturnStatus = transaction.ReturnStatus;
            supplierWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, supplierWebApi);
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody] List<SupplierDTO> suppliers)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            SupplierApiModels supplierWebApi = new SupplierApiModels();
            try
            {
                suppliers.ForEach(c =>
                {
                    Supplier model = this.ImsService.GetSupplier(c.ID);
                    model.Name = c.Name;
                    model.Address = c.Address;
                    model.Contact = c.Contact;
                    model.ContactPhone = c.ContactPhone;
                    model.CS = c.CS;
                    model.CSPhone = c.CSPhone;
                    model.Platform = c.Platform;
                    model.ID = c.ID;
                    model.URL = c.URL;
                    model.UserId = c.UserId;
                    this.ImsService.SaveSupplier(model);
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
                supplierWebApi.ReturnMessage = transaction.ReturnMessage;
                supplierWebApi.ReturnStatus = transaction.ReturnStatus;
                supplierWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, supplierWebApi);
                return badResponse;
            }
            string returnMessage = "批量修改成功";
            supplierWebApi.IsAuthenicated = true;
            supplierWebApi.ReturnStatus = transaction.ReturnStatus;
            supplierWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, supplierWebApi);
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> SupplierID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            SupplierApiModels supplierWebApi = new SupplierApiModels();
            try
            {
                this.ImsService.DeleteSupplier(SupplierID);
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
                supplierWebApi.ReturnMessage = transaction.ReturnMessage;
                supplierWebApi.ReturnStatus = transaction.ReturnStatus;
                supplierWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, supplierWebApi);
                return badResponse;
            }
            string returnMessage = "批量删除成功";
            supplierWebApi.IsAuthenicated = true;
            supplierWebApi.ReturnStatus = transaction.ReturnStatus;
            supplierWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, supplierWebApi);
            
        }
    }
}
