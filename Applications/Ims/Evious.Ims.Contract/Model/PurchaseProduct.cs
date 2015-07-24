using Evious.Framework.Contract;
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
    [Table("PurchaseProducts")]
    public class PurchaseProduct 
    {
        public int PurchaseID { get; set; }

        public int ProductID { get; set; }

        [DisplayName("采购数量")]
        public int Quantity { get; set; }

        [DisplayName("采购单价")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }


        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        [ForeignKey("PurchaseID")]
        public virtual Purchase Purchase { get; set; }
    }
}
