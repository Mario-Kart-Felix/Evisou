using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using Evisou.Framework.Utility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evisou.Ims.Contract.Model
{
    [Auditable]
    [Table("Supplier")]
    public class Supplier : ModelBase
    {
        public Supplier() 
        {

        }

        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("平台")]
        public string Platform { get; set; }

        [DisplayName("供应商")]
        public string Name { get; set; }
        [Url]
        [DisplayName("网址")]
        public string URL { get; set; }

        [DisplayName("客服")]
        public string CS { get; set; }

        [DisplayName("客服电话")]
        [DataType(DataType.PhoneNumber)]
        public string CSPhone { get; set; }

        [DisplayName("联系人")]
        public string Contact { get; set; }

        [DisplayName("联系人电话")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        public virtual List<Product> Products { get; set; }

        [NotMapped]
        [Required]
        [DisplayName("供应的产品")]
        public int[] SelectedProduct
        {
            get
            {
                if (Products != null)
                {
                    return Products.Select(t => t.ID).ToArray<int>();
                }
                else
                { 
                  return new int[]{};
                }
                    
            }
            set
            {
                if (value != null)
                {
                    if (value.Count<int>() > 0)
                    {
                        this.Products = value.Select(t => new Product() { ID = t }).ToList();
                    }
                    else
                    {
                        this.Products = new List<Product>();
                    }
                }
                else
                {
                    this.Products = new List<Product>();
                } 
            }
        }
        

       
    }
}
