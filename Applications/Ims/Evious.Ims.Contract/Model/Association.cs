using Evious.Framework.Contract;
using Evious.Ims.Contract.Enum;
using Evious.Ims.Contract.Model.PayPal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model
{
    [Auditable]
    [Table("Association")]
    public class Association : ModelBase
    {
        public int ProductID { get; set; }

        public int PaypalApiID { get; set; }
        public int UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("标题")]
        [StringLength(160)]
        public string ItemTitle { get; set; }

        [DisplayName("序号")]
        [StringLength(160)]
        public string ItemNumber { get; set; }


        [DisplayName("储存地点")]
        public string StorePlace { get; set; }

        [DisplayName("销售地点")]
        public string SellingPlace { get; set; }

        public virtual PaypalApi PaypalApi { get; set; }

        public virtual Product Product { get; set; }

        public List<TransactionItem> TransactionItems { get; set; }

        public List<PaymentItemType> PaymentItems { get; set; }

        public List<PayPalTransactionPaymentItem> PayPalTransactionPaymentItems { get; set; }
    }
}
