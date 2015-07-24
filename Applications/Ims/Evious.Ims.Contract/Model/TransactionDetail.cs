using Evious.Framework.Contract;
using Evious.Ims.Contract.Enum;
using Newtonsoft.Json;
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
    [Table("TransactionDetail")]
    public class TransactionDetail : ModelBase
    {
        public TransactionDetail()
        { 
        
        }

        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }


        [DisplayName("接收商业")]
        [EmailAddress]
        public string ReceiverBusiness { get; set; }
        

        [DisplayName("收款ID")]
        public string ReceiverID { get; set; }

        //[Required]
        [EmailAddress]
        [DisplayName("收款电邮")]
        public string ReceiverEmail { get; set; }


        [DisplayName("交易ID")]
        public string TransactionID { get; set; }


        [DisplayName("上级交易ID")]
        public string ParentTransactionID { get; set; }


        [DisplayName("交易类型")]
        public string TransactionType { get; set; }


        [DisplayName("快递名称")]
        public string PaymentType { get; set; }


        [DisplayName("交易时间")]
        public DateTime OrderTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        [DisplayName("AMT")]
        public decimal Amt { get; set; }


        [DisplayName("FEEAMT")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        public decimal FeeAmt { get; set; }


        [DisplayName("货币代码")]
        public string CurrencyCode { get; set; }


        [DisplayName("付款状态")]
        public string PaymentStatus { get; set; }


        [DisplayName("冻结原因")]
        public string PendingReason { get; set; }


        [DisplayName("原因代码")]
        public string ReasonCode { get; set; }


        [DisplayName("合法保护")]
        public string ProtectionEligibility { get; set; }


        [DisplayName("税AMT")]
        [Column(TypeName = "money")]
        public decimal TaxAmt { get; set; }


        [DisplayName("电邮")]
        [EmailAddress]
        public string Email { get; set; }


        [DisplayName("付款ID")]
        public string PayerID { get; set; }


        [DisplayName("付款者状态")]
        public string PayerStatus { get; set; }


        [DisplayName("国家代码")]
        public string CountryCode { get; set; }


        [DisplayName("收货人")]
        public string ShiptoName { get; set; }


        [DisplayName("街道")]
        public string ShipToStreet { get; set; }


        [DisplayName("街道2")]
        public string ShipToStreet2 { get; set; }


        [DisplayName("城市")]
        public string ShiptoCity { get; set; }


        [DisplayName("州/省")]
        public string ShipToState { get; set; }


        [DisplayName("国家代码")]
        public string ShipToCountryCode { get; set; }


        [DisplayName("国家名称")]
        public string ShipToCountryName { get; set; }


        [DisplayName("邮编")]
        public string ShipToZip { get; set; }


        [DisplayName("全地址")]
        public string FullAddress
        {
            get
            {
                return ShiptoName + ", " + ShipToStreet + ", " + ShipToStreet2 + ", " + ShiptoCity + ", " + ShipToState + ", " + ShipToZip + ", " + ShipToCountryName;
            }
        }


        [DisplayName("AddressOwner")]
        public string AddressOwner { get; set; }


        [DisplayName("地址状态")]
        public string AddressStatus { get; set; }


        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        [DisplayName("销售税")]
        public decimal SaleTax { get; set; }


        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        [DisplayName("运费")]
        public decimal ShipAmount { get; set; }


        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        [DisplayName("处理费")]
        public decimal ShipHandleAmount { get; set; }


        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        [DisplayName("保险费")]
        public decimal InsuranceAmount { get; set; }


        [DisplayName("标题")]
        public string Subject { get; set; }


        [DisplayName("买家ID")]
        public string BuyerID { get; set; }

        [DisplayName("名")]
        public string FirstName { get; set; }


        [DisplayName("姓")]
        public string LastName { get; set; }


        [DisplayName("备注")]
        public string Remark { get; set; }


        [DisplayName("跟踪号")]
        public string TrackingNumber { get; set; }

        [DisplayName("物流代理")]
        public ExpressAgentEnum Agent { get; set; }

        [DisplayName("快递服务")]
        public string Express { get; set; }

        [DisplayName("物流代理单号")]
        public string OrderSign { get; set; }

        [DisplayName("运费")]
        public decimal AgentPostage { get; set; }
       // [JsonIgnore]
        public List<TransactionItem> TransactionItems { get; set; }
    }
}
