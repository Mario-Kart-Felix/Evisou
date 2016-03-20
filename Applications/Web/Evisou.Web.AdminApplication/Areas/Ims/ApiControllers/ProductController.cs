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
        
        public HttpResponseMessage GetAllProducts()
        {
            TransactionalInformation transaction = new TransactionalInformation();

            IEnumerable<Product> allProduct = this.ImsService.GetProductList(null).Where(i=>i.ID==6);
            IEnumerable<Product> filterProduct = allProduct;

            


            ProductsApiModel productWebApi = new ProductsApiModel();
            productWebApi.Products = allProduct.ToList();
            productWebApi.ReturnMessage = transaction.ReturnMessage;
            productWebApi.ReturnStatus = transaction.ReturnStatus;
            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, productWebApi);
                return response;
            }

            var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, productWebApi);
            return badResponse;
        }
    }
}
