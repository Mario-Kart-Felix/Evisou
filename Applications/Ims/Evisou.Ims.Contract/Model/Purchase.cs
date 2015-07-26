using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    [Auditable]
    [Table("Purchase")]
    public class Purchase : ModelBase
    {
        public Purchase()
        { 
        
        }
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("采购日期")]
        public DateTime? PurchaseDate { get; set; }

        [DisplayName("采购交易号")]
        public string PurchaseTransactionID { get; set; }

        public int SupplierID { get; set; }

        [DisplayName("供应商")]
        public virtual Supplier Supplier { get; set; }
           
        [NotMapped]
        public int[] selectedProduct { get; set; }

        [NotMapped]
        public decimal[] selectedPrice { get; set; }

        [NotMapped]
        public int[] selectedQuantity { get; set; }    
        
        public virtual List<PurchaseProduct> PurchaseProducts { get; set; }

    }
}
