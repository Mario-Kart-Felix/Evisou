using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Evisou.Web.AdminApplication.Common;
using Evisou.Ims.Contract.Model;
using Evisou.Account.Contract;
using Evisou.Web.AdminApplication.Areas.Ims.WebApiModels;

namespace Evisou.Web.AdminApplication.Areas.Ims.ApiControllers
{
    
    [WebApiPermission(EnumBusinessPermission.ImsManage_Product)]
    public class ProductController:AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] ProductInquiryDTO productInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsApiModel productWebApi = new ProductsApiModel();

            try
            {
                var all = this.ImsService.GetProductList(null);
                IEnumerable<Product> filter = all;
                if (!string.IsNullOrEmpty(productInquiryDTO.Name))
                    filter = filter.Where(c => c.Name.Contains(productInquiryDTO.Name));
                if (!string.IsNullOrEmpty(productInquiryDTO.Sku))
                    filter = filter.Where(c => c.Sku.Contains(productInquiryDTO.Sku));

                int start = (productInquiryDTO.CurrentPageNumber - 1) * productInquiryDTO.PageSize;
                string sortDirection = productInquiryDTO.SortDirection;
                string sortExpression = productInquiryDTO.SortExpression;
                if (productInquiryDTO.PageSize > 0)
                    filter = filter.Skip(start).Take(productInquiryDTO.PageSize);

                List<ProductDTO> productList = new List<ProductDTO>();
                filter.ToList().ForEach(c =>
                {
                    productList.Add(new ProductDTO
                    {
                        Name = c.Name,
                        NetSize = c.NetSize,
                        PackingSize = c.PackingSize,
                        PackSize = c.PackSize,
                        PackWeight = c.PackWeight,
                        Remark = c.Remark,
                        Size = c.Size,
                        Sku = c.Sku,
                        Weight = c.Weight,
                        ID = c.ID
                    });
                });

                Func<ProductDTO, string> orderingFunction = (
                        c => sortExpression.Contains("Sku") ? c.Sku :
                        sortExpression.Contains("Name") ? c.Name :
                        ""
                    );
                IEnumerable<ProductDTO> Result = new List<ProductDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = productList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = productList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = productList;
                        break;

                };
                productWebApi.Products = Result;
                productWebApi.TotalRecords = all.Count();
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
                productWebApi.ReturnMessage = transaction.ReturnMessage;
                productWebApi.ReturnStatus = transaction.ReturnStatus;
                productWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, productWebApi);
                return badResponse;
            }
            productWebApi.ReturnMessage = transaction.ReturnMessage;
            productWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, productWebApi);
        }

        [HttpGet]
        public HttpResponseMessage GetOne([FromUri] int AgentID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsApiModel productWebApi = new ProductsApiModel();
            try
            {
                var model = this.ImsService.GetProduct(AgentID);
                productWebApi.Product = new ProductDTO
                {
                    Name = model.Name,
                    NetSize = model.NetSize,
                    PackingSize = model.PackingSize,
                    PackSize = model.PackSize,
                    PackWeight = model.PackWeight,
                    Remark = model.Remark,
                    Size = model.Size,
                    Sku = model.Sku,
                    Weight = model.Weight
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
                productWebApi.ReturnMessage = transaction.ReturnMessage;
                productWebApi.ReturnStatus = transaction.ReturnStatus;
                productWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, productWebApi);
                return badResponse;
            }
            productWebApi.ReturnMessage = transaction.ReturnMessage;
            productWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, productWebApi);
        }

        [HttpGet]
        public HttpResponseMessage Export()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]ProductDTO ProductDTO)
        {
            Product model = new Product
            {
                Name = ProductDTO.Name,
                NetSize = ProductDTO.NetSize,
                PackingSize = ProductDTO.PackingSize,
                PackSize = ProductDTO.PackSize,
                PackWeight = ProductDTO.PackWeight,
                Remark = ProductDTO.Remark,
                Size = ProductDTO.Size,
                Sku = ProductDTO.Sku,
                Weight = ProductDTO.Weight,
                ID = ProductDTO.ID
            };
            return SaveProduct(model, "添加成功");
            
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]ProductDTO ProductDTO)
        {
            Product model = this.ImsService.GetProduct(ProductDTO.ID);
            model.Name = ProductDTO.Name;
            model.NetSize = ProductDTO.NetSize;
            model.PackingSize = ProductDTO.PackingSize;
            model.PackSize = ProductDTO.PackSize;
            model.PackWeight = ProductDTO.PackWeight;
            model.Remark = ProductDTO.Remark;
            model.Size = ProductDTO.Size;
            model.Sku = ProductDTO.Sku;
            model.Weight = ProductDTO.Weight;
            model.ID = ProductDTO.ID;
            return SaveProduct(model, "修改成功");
        }
        private HttpResponseMessage SaveProduct(Product model, string returnMessage)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsApiModel productWebApi = new ProductsApiModel();
            try
            {
                this.ImsService.SaveProduct(model);
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
                productWebApi.ReturnMessage = transaction.ReturnMessage;
                productWebApi.ReturnStatus = transaction.ReturnStatus;
                productWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, productWebApi);
                return badResponse;
            }

            productWebApi.IsAuthenicated = true;
            productWebApi.ReturnStatus = transaction.ReturnStatus;
            productWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, productWebApi);
        }
        [HttpPatch]
        public HttpResponseMessage Patch([FromBody]List<ProductDTO> products)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsApiModel productWebApi = new ProductsApiModel();
            try
            {
                products.ForEach(c =>
                {
                    Product model = this.ImsService.GetProduct(c.ID);
                    model.Name = c.Name;
                    model.NetSize = c.NetSize;
                    model.PackingSize = c.PackingSize;
                    model.PackSize = c.PackSize;
                    model.PackWeight = c.PackWeight;
                    model.Remark = c.Remark;
                    model.Size = c.Size;
                    model.Sku = c.Sku;
                    model.Weight = c.Weight;
                    model.ID = c.ID;
                    this.ImsService.SaveProduct(model);
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
                productWebApi.ReturnMessage = transaction.ReturnMessage;
                productWebApi.ReturnStatus = transaction.ReturnStatus;
                productWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, productWebApi);
                return badResponse;
            }

            string returnMessage = "批量修改成功";
            productWebApi.IsAuthenicated = true;
            productWebApi.ReturnStatus = transaction.ReturnStatus;
            productWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, productWebApi);
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> ProductID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsApiModel productWebApi = new ProductsApiModel();
            try
            {
                this.ImsService.DeleteProduct(ProductID);
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
                productWebApi.ReturnMessage = transaction.ReturnMessage;
                productWebApi.ReturnStatus = transaction.ReturnStatus;
                productWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, productWebApi);
                return badResponse;
            }

            string returnMessage = "批量删除成功";
            productWebApi.IsAuthenicated = true;
            productWebApi.ReturnStatus = transaction.ReturnStatus;
            productWebApi.ReturnMessage.Add(returnMessage);
            return Request.CreateResponse(HttpStatusCode.OK, productWebApi);
        }

    }
}
