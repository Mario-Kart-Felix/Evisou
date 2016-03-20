using Evisou.Ims.Contract.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class ProductsApiModel : TransactionalInformation
    {
        public List<Product> Products;
        public Product Product;

        public ProductsApiModel()
        {
            Product = new Product();
            Products = new List<Product>();
        }

     }

    public class ProductDTO
    {
        [DisplayName("产品SKU")]
        [Required]
        [MaxLength(10)]
        public string Sku { get; set; }

        [DisplayName("产品名称")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [DisplayName("净尺寸")]
        public string Size { get; set; }

        [DisplayName("净尺寸")]
        public Packing NetSize { get; set; }

        [DisplayName("净重量")]
        public int Weight { get; set; }

        [DisplayName("毛尺寸")]
        // [Required]
        [MaxLength(100)]
        public string PackSize { get; set; }

        [DisplayName("毛尺寸")]
        public Packing PackingSize { get; set; }

        [DisplayName("毛重")]
        //[Required]
        public int PackWeight { get; set; }

        [DisplayName("备注")]
        [StringLength(int.MaxValue)]
        public string Remark { get; set; }


    }

    public class ProductInquiryDTO
    {
        public string Sku { get; set; }

        public string Name { get; set; }
    }
}