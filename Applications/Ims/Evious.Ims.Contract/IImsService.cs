using Evious.Ims.Contract.Model;
using Evious.Ims.Contract.Model.PayPal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evious.Ims.Contract
{
    public interface IImsService : IPayPalHelper
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetProductList(ProductRequest request = null);
        void SaveProduct(Product product);
        void DeleteProduct(List<int> ids);
        void Datatable();
        Image GetImage(string imagepath);
        void DeleteImage(string imagepath);


        Supplier GetSupplier(int id);
        IEnumerable<Supplier> GetSupplierList(SupplierRequest request = null);
        void SaveSupplier(Supplier supplier);
        void DeleteSupplier(List<int> ids);


        Purchase GetPurchase(int id);
        IEnumerable<Purchase> GetPurchaseList(PurchaseRequest request = null);
        void SavePurchase(Purchase purchase);
        void DeletePurchase(List<int> ids);
        IEnumerable<Product> getProductBySupplier(int id, int? purchaseid);


        Agent GetAgent(int id);
        IEnumerable<Agent> GetAgentList(AgentRequest request = null);
        void SaveAgent(Agent agent);
        void DeleteAgent(List<int> ids);

        PayPalTransaction GetPayPalTransaction(int id);
        IEnumerable<PayPalTransaction> GetPayPalTransactionList(PayPalTransactionRequest request = null);

        Task<List<PayPalTransaction>> GetPayPalTransactionListIQueryable(PayPalTransactionRequest request = null);
        Task<IEnumerable<PayPalTransaction>> GetPayPalTransactionListAsync(PayPalTransactionRequest request = null);

        Task<IEnumerable<PayPalTransaction>> GetPayPalTransactionListAsync2(PayPalTransactionRequest request = null);
        void SavePayPalTransaction(PayPalTransaction paypaltransaction);
        void DeletePayPalTransaction(List<int> ids);
        void PayPalTransactionDataExport(List<int> ids, string type);

        Evious.Ims.Contract.Model.PayPal.PaymentTransactionType GetPaymentTransaction(int id);
        IEnumerable<Evious.Ims.Contract.Model.PayPal.PaymentTransactionType> GetPaymentTransactionList(PaymentTransactionRequest request = null);
        void SavePaymentTransaction(Evious.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction);
        void DeletePaymentTransaction(List<int> ids);
        List<PaymentItemType> DistinctPaymentItemInfo();


        TransactionDetail GetTransactionDetail(int id);
        IEnumerable<TransactionDetail> GetTransactionDetailList(TransactionDetailRequest request = null);
        void SaveTransactionDetail(TransactionDetail transactiondetail);
        void DeleteTransactionDetail(List<int> ids);
        List<TransactionItem> DistinctTransactionItem();
        List<TransactionDetail> GetCountryNameAndCode();

        PaypalApi GetPaypalApi(int id);
        IEnumerable<PaypalApi> GetPaypalApiList(PaypalApiRequest request = null);
        void SavePaypalApi(PaypalApi paypalapi);
        void DeletePaypalApi(List<int> ids);

        Association GetAssociation(int id);
        IEnumerable<Association> GetAssociationList(AssociationRequest request = null);
        void SaveAssociation(Association association);
        void DeleteAssociation(List<int> ids);


        IEnumerable<Express> GetDriectExpress(int agentid);
        IEnumerable<Express> GetOutboundExpress(int agentid,string warehouse);

        ShipOrder AddDispatchOrder(DispatchRequest request);

        ShipOrder AddDispatchOrder(DispatchRequest request, Evious.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction);

        ShipOrder AddDispatchOrder(DispatchRequest tranasctiondetail, PayPalTransaction model);

        string DeleteDispatchOrder(int id);

        string SubmitDispatchOrder(int id);


        string DeleteDispatchOrder(Evious.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction);

        string SubmitDispatchOrder(Evious.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction);


        string DeleteDispatchOrder(PayPalTransaction paypaltransaction);

        string SubmitDispatchOrder(PayPalTransaction paypaltransaction);
        IEnumerable<Evious.Core.Log.AuditLog> GetAuditLogList(AuditLogRequest request = null);




        
    }
}
