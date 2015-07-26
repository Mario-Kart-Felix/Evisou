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
    [Table("Product")]
    public class Product : ModelBase
    {
        public Product() { }

        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

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

        [DisplayName("产品图片")]
        [StringLength(300)]
        public string CoverPicture { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public bool Assigned { get; set; }

        public virtual  List<Supplier> Suppliers {get; set; }

        [DisplayName("详细描述")]
        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        [DisplayName("短描述")]
        public string ShortDescription { get; set; }

        [DisplayName("是否公开")]
        public bool Status { get; set; }


        [DisplayName("采购单价")]
        [DataType(DataType.Currency)]
        public decimal? SellingPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("可用日期开始")]
        public DateTime? AvailableFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("可用日期结束")]
        public DateTime? AvailableTo { get; set; }

        #region Meta
        [StringLength(100)]
        public string MetaTitle { get; set; }

        [StringLength(1000)]
        public string MetaKeywords { get; set; }

        [StringLength(255)]
        public string MetaDescription { get; set; }


        #endregion

        #region Images

        [NotMapped]
        public string[] PictureURL { get; set; }

        [NotMapped]
        public string[] Label { get; set; }

        [NotMapped]
        public int[] SortOrder { get; set; }

        public int[] PictureID { get; set; }
        public List<Image> Images { get; set; }

        [NotMapped]
        [Required]
        [DisplayName("和产品相关的图片")]
        public int[] SelectedImages
        {
            get
            {
                if (Images != null)
                {
                    return Images.Select(t => t.ID).ToArray<int>();
                }
                else
                {
                    return new int[] { };
                }

            }
            set
            {
                if (value != null)
                {
                    if (value.Count<int>() > 0)
                    {
                        this.Images = value.Select(t => new Image() { ID = t }).ToList();
                    }
                    else
                    {
                        this.Images = new List<Image>();
                    }
                }
                else
                {
                    this.Images = new List<Image>();
                }
            }
        }
        

        #endregion

        #region Reviews
        public List<Review> Reviews { get; set; }

        #endregion
    }
}
