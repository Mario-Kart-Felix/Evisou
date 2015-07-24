using Evious.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalPersonName")]
   public class PersonNameType:ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Salutation { get; set; }
        public string Suffix { get; set; }
    }
}
