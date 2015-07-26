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
    [Table("PayPalPaymentInfo")]
    public class PaymentInfoType : ModelBase
    {
        
        public string BinEligibility { get; set; }
        public string EbayTransactionID { get; set; }
        //public EnhancedPaymentInfoType EnhancedPaymentInfo { get; set; }
        public string ExchangeRate { get; set; }
        public string ExpectedeCheckClearDate { get; set; }

     /*   public int FeeAmountID { get; set; }
        public BasicAmountType FeeAmount { get; set; }*/

       // public FMFDetailsType FMFDetails { get; set; }

        //public int GrossAmountID { get; set; }
      //  public BasicAmountType GrossAmount { get; set; }
        public string HoldDecision { get; set; }

        public int InstrumentDetailsID { get; set; }
        public InstrumentDetailsType InstrumentDetails { get; set; }
        public string InsuranceAmount { get; set; }
       // public OfferDetailsType OfferDetails { get; set; }
        public string ParentTransactionID { get; set; }
        public string PaymentDate { get; set; }
       // public ErrorType PaymentError { get; set; }
        public string PaymentRequestID { get; set; }
        public PaymentStatusCodeType? PaymentStatus { get; set; }
        public PaymentCodeType? PaymentType { get; set; }
        public PendingStatusCodeType? PendingReason { get; set; }
        public POSTransactionCodeType? POSTransactionType { get; set; }
        public string ProtectionEligibility { get; set; }
        public string ProtectionEligibilityType { get; set; }
        public ReversalReasonCodeType? ReasonCode { get; set; }
        public string ReceiptID { get; set; }
        public string ReceiptReferenceNumber { get; set; }
        public RefundSourceCodeType? RefundSourceCodeType { get; set; }

        public int SellerDetailsID { get; set; }
        public SellerDetailsType SellerDetails { get; set; }

       // public BasicAmountType SettleAmount { get; set; }
        public string ShipAmount { get; set; }
        public string ShipDiscount { get; set; }
        public string ShipHandleAmount { get; set; }
        public string ShippingMethod { get; set; }
        public string StoreID { get; set; }
        public string Subject { get; set; }

      //  public int TaxAmountID { get; set; }
      //  public BasicAmountType TaxAmount { get; set; }
        public string TerminalID { get; set; }
        public string TransactionID { get; set; }
        public PaymentTransactionCodeType? TransactionType { get; set; }

    }
}
