using Evious.Framework.Contract;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
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
        /*   public int SecondaryAddressID { get; set; }
        public AddressType SecondaryAddress { get; set; }
        public List<string> SurveyChoiceSelected { get; set; }
        public string SurveyQuestion { get; set; }
        public string TPLReferenceID { get; set; }

        public int UserSelectedOptionsID { get; set; }
        public UserSelectedOptionType UserSelectedOptions { get; set; }*/
        #endregion


        #region 付款者信息（对应PaymentTransactionType.PayerInfoType）
        public int PayerInfoID { get; set; }
        /// <summary>
        /// 付款者信息（对应PaymentTransactionType.PayerInfoType）
        /// </summary>
        public PayerInfoType PayerInfo { get; set; }

        #endregion

     /*   #region 付款信息（对应PaymentTransactionType.PaymentInfoType）
        public int PaymentInfoID { get; set; }

        /// <summary>
        /// 付款信息（对应PaymentTransactionType.PaymentInfoType）
        /// </summary>
        public PaymentInfoType PaymentInfo { get; set; }

        #endregion

        #region  付款商品信息（对应PaymentTransactionType.PaymentItemInfoType）
        public int PaymentItemInfoID { get; set; }

        /// <summary>
        /// 付款商品信息（对应PaymentTransactionType.PaymentItemInfoType）
        /// </summary>
        public PaymentItemInfoType PaymentItemInfo { get; set; }
        #endregion

        #region 收款者信息（对应PaymentTransactionType.ReceiverInfoType）
        public int ReceiverInfoID { get; set; }

        /// <summary>
        /// 收款者信息（对应PaymentTransactionType.ReceiverInfoType）
        /// </summary>
        public ReceiverInfoType ReceiverInfo { get; set; }
        #endregion*/
    }
       
}
