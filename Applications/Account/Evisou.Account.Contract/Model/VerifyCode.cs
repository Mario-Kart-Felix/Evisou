using System;
using System.Linq;
using Evisou.Framework.Contract;
using System.Collections.Generic;
using Evisou.Framework.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evisou.Account.Contract
{
    [Serializable]
    [Table("VerifyCode")]
    public class VerifyCode : ModelBase
    {
        public Guid Guid { get; set; }
        public string VerifyText { get; set; }
    }

}



