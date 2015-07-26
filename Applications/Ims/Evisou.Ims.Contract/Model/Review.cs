using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    [Serializable]
    [Table("Review")]
    public class Review : ModelBase
    {
        public Review() { }

        public int ProductID { get; set; }

        [DisplayName("评论日期")]
        public DateTime ReviewDate { get; set; }
        [DisplayName("评论内容")]
        public string ReviewContent { get; set; }


        /// <summary>
        /// 0:pending,1:Approved,2:Rejected
        /// </summary>
        [DisplayName("状态")]
        public int Status { get; set; }

       // public string Buyer { get; set; }

    }
}
