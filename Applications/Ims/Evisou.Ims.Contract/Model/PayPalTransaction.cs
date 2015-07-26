using Evisou.Framework.Contract;
using Evisou.Ims.Contract.Enum;
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
    [Table("PayPalTransaction")]
    public class PayPalTransaction : ModelBase
    {
       

        public string Receiver { get; set; }

        public string PayerID { get; set; }

        public string Payer { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerMiddleName { get; set; }
        public string PayerLastName { get; set; }
        public string PayerBusiness { get; set; }
        public string PayerAddressOwner { get; set; }
        public string PayerAddressStatus { get; set; }
        public string PayerAddressName { get; set; }

        public string PayerAddressStreet1 { get; set; }

        public string PayerAddressStreet2 { get; set; }
        public string PayerCityName { get; set; }
        public string PayerStateOrProvince { get; set; }
        public string PayerPostalCode { get; set; }
        public string PayerCountry { get; set; }

        public string PayerCountryName { get; set; }

        public string PayerPhone { get; set; }

        public string TransactionId { get; set; }

        public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public string SettleAmountCurrencyId { get; set; }
        public string SettleAmountValue { get; set; }
        public decimal TaxAmount { get; set; }


        public string ExchangeRate { get; set; }
        public string PaymentStatus { get; set; }
        public string PendingReason { get; set; }
        public string InvoiceID { get; set; }
        public string Memo { get; set; }
        public string SaleTax { get; set; }
        public string PayerStatus { get; set; }
        public List<PayPalTransactionPaymentItem> PayPalTransactionPaymentItems { get; set; }
        #region 物流信息

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

        #endregion



        public string CurrencyCode { get; set; }

        public string Subject { get; set; }

        public string BuyerID { get; set; }

        public string ParentTransactionID { get; set; }
    }
}
