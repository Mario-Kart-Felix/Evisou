using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class TransactionDetailApiModels : TransactionalInformation
    {
        public IEnumerable<TransactionDetailDTO> TransactionDetails;
        public TransactionDetailDTO TransactionDetail;
        public TransactionDetailApiModels()
        {
            TransactionDetail = new TransactionDetailDTO();
            TransactionDetails = new List<TransactionDetailDTO>();

        }
    }

    public class TransactionDetailDTO : ModelBase
    {
        public string Receiver { get; set; }

        public string PayerID { get; set; }

        public string Payer { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerMiddleName { get; set; }
        public string PayerLastName { get; set; }

        public string TransactionId { get; set; }
    }

    public class TransactionDetailInquiryDTO : InquiryDTO
    {
        public string TransactionId { get; set; }
    }
}