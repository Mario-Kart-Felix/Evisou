using Evious.Account.Contract;
using Evious.Ims.Contract.Model;
//using Evious.Web.AdminAPI.Models;
using Evious.Web.AdminAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Evious.Web.AdminAPI.Controllers
{
    public class ProductsController : AdminApiControllerBase
    {

        #region


        #endregion



        
        public IEnumerable<Product> GetAllProducts()
        {

            var products = this.ImsService.GetProductList(null).AsEnumerable<Product>();
            
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
           var product = this.ImsService.GetProductList(null).FirstOrDefault((p) => p.ID == id);
            //var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        
      /*
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        */

    }
}