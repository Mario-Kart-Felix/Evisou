using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    public class History : ModelBase
    {
        public History() { }

        public DateTime DateTime { get; set; }

        [DisplayName("详细描述")]
        [StringLength(int.MaxValue)]
        public string Description { get; set; }
    }
}
