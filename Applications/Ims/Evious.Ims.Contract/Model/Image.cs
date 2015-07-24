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
    [Table("Image")]
    public class Image : ModelBase
    {
        public Image()
        {
        
        }
        public virtual List<Product> Products { get; set; }
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("图片Label")]
       // [Required]
        public string Label { get; set; }

        [DisplayName("图片类型")]//0:Base Image,1:Small Image,2:Thumbnail
        //[Required]
        public int ImageType { get; set; }

        [DisplayName("图片排序")]
        public int SortOrder { get; set; }

        [DisplayName("产品图片")]
        [StringLength(300)]
        public string PictureURL { get; set; }
    }
}
