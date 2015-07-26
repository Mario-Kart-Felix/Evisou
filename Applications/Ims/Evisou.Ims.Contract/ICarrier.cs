using Evisou.Ims.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Evisou.Ims.Contract
{
    public interface ICarrier
    {
        ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, TransactionDetail transactiondetail);

        ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, PayPalTransaction paypaltransaction);
        string DeleteExpressOrder(TransactionDetail transactiondetail);
        string SubmitExpressOrder(TransactionDetail transactiondetail);
        IEnumerable<Express> GetExpressList();

        SummaryInfo GetDirectExpressShippingFee(PriceRequest requst);
    }
}
