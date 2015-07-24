using Evious.Framework.Contract;
using Evious.Ims.Contract.Enum;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalPaymentTransaction")]
    public class PaymentTransactionType : ModelBase
    {
        #region
        /*public string BuyerEmailOptIn { get; set; }
        public string GiftMessage { get; set; }
        public string GiftReceipt { get; set; }

        public int GiftWrapAmountID { get; set; }
        public BasicAmountType GiftWrapAmount { get; set; }
        public string GiftWrapName { get; set; }
        public OfferCouponInfoType OfferCouponInfo { get; set; }*/
        #endregion

        #region        
       /* public AddressType SecondaryAddress { get; set; }
        public List<string> SurveyChoiceSelected { get; set; }
        public string SurveyQuestion { get; set; }
        public string TPLReferenceID { get; set; }

        public int UserSelectedOptionsID { get; set; }
        public UserSelectedOptionType UserSelectedOptions { get; set; }*/
        #endregion


        #region 付款者信息（对应PaymentTransactionType.PayerInfoType）
       // public int PayerInfoID { get; set; }
        /// <summary>
        /// 付款者信息（对应PaymentTransactionType.PayerInfoType）
        /// </summary>
        public PayerInfoType PayerInfo { get; set; }

        #endregion

        #region 付款信息（对应PaymentTransactionType.PaymentInfoType）
        /// <summary>
        /// 付款信息（对应PaymentTransactionType.PaymentInfoType）
        /// </summary>
        public PaymentInfoType PaymentInfo { get; set; }

        #endregion

        #region  付款商品信息（对应PaymentTransactionType.PaymentItemInfoType）
                /// <summary>
        /// 付款商品信息（对应PaymentTransactionType.PaymentItemInfoType）
        /// </summary>
        public PaymentItemInfoType PaymentItemInfo { get; set; }
        #endregion

        #region 收款者信息（对应PaymentTransactionType.ReceiverInfoType）      

        /// <summary>
        /// 收款者信息（对应PaymentTransactionType.ReceiverInfoType）
        /// </summary>
        public ReceiverInfoType ReceiverInfo { get; set; }
        #endregion


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
    }
       
}
