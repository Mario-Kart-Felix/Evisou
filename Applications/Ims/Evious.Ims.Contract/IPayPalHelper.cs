using Evious.Ims.Contract.Model;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;

namespace Evious.Ims.Contract
{
    public interface IPayPalHelper
    {
        IEnumerable<PaymentTransactionSearchResultType> ApiTransactionSearch(DateTime startDate, DateTime endDate);

        GetTransactionDetailsResponseType ApiTransactionDetail(PaymentTransactionSearchResultType transctionDetail);
        Evious.Ims.Contract.Model.PayPal.PaymentTransactionType PaymentTransaction(GetTransactionDetailsResponseType getTransactionDetail);
        TransactionDetail TransactionDetail(GetTransactionDetailsResponseType getTransactionDetail);

        PayPalTransaction PayPayTransaction(GetTransactionDetailsResponseType getTransactionDetail);
        void PayPalHelper(PaypalApi paypalapi);
        void PayPalHelper();
    }
}
