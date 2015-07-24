using Evious.Ims.Contract.Model;
using Evious.Web.AdminAPI.Common;
using Evious.Web.AdminAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evious.Web.AdminAPI.Controllers
{
    public class OrdersController : AdminApiControllerBase
    {
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<OrderDTO> Get()
        {
            var transactiondetails = this.ImsService.GetPayPalTransactionList(null).ToList().Select(
                p => new OrderDTO
                {
                    ID=p.ID,
                    TransactionID = p.TransactionId,
                    TransactionItems = p.PayPalTransactionPaymentItems.Select(x => new PayPalTransactionPaymentItem
                    {
                         PaymentItemEbayItemTxnId=x.PaymentItemEbayItemTxnId,
                          PaymentItemName=x.PaymentItemName,
                           PaymentItemNumber=x.PaymentItemNumber,
                            PaymentItemQuantity=x.PaymentItemQuantity
                        
                    }).ToList()
                });

            return transactiondetails;
        }
        //public IEnumerable<Product> GetAllOrders()
        //{

        //    var products = this.ImsService.GetProductList(null).AsEnumerable<Product>();

        //    return products;
        //}

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var transactiondetail = this.ImsService.GetTransactionDetailList(null).FirstOrDefault((p) => p.ID == id);
            if (transactiondetail == null)
            {
                return NotFound();
            }
            return Ok(transactiondetail);
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
    }
}