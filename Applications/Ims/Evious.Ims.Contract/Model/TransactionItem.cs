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
    [Serializable]
    [Table("TransactionItem")]
    public class TransactionItem : ModelBase
    {

        public TransactionItem()
       { 
       
       }
        public int TransactionDetailID { get; set; }

        
        public string ItemID { get; set; }

        [DisplayName("标题")]
        [StringLength(160)]
        public string ItemTitle { get; set; }

        [DisplayName("货物数量")]
        public int ItemQty { get; set; }

        [DisplayName("ItemAmt")]
        [Column(TypeName = "money")]
        public decimal ItemAmt { get; set; }

        public virtual TransactionDetail TransactionDetail { get; set; }

        public int? AssociationID { get; set; }

        [InverseProperty("TransactionItems")]
        public virtual Association Association{ get; set; }

        [NotMapped]
        public string CountryCode { get; set; }
    }
}
