using Evious.Ims.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evious.Web.AdminAPI.Models
{
    public class OrderDTO
    {
        public int ID { set; get; }

        public string TransactionID { get; set; }

        public List<PayPalTransactionPaymentItem> TransactionItems { get; set; }
    }
}