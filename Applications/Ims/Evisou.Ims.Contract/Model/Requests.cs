using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    public class ProductRequest : Request
    {
        //public int ID { get; set; }

        public List<int> IDs { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
    }

    public class SupplierRequest : Request
    {
        public string Name { get; set; }
       // public int ID { get; set; }   
    }

    public class PurchaseRequest : Request
    {
        public string PurchaseTransactionID { get; set; }  
    }

    public class AgentRequest : Request
    {
       // public int ID { get; set; }

        public string AgentName { get; set; }   
    }

    public class PaymentTransactionRequest : Request
    {
       // public int ID { get; set; }

        public string PaymentStatus { get; set; }
        public string TransactionID { get; set; }

        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        public string sAction { get; set; }
    }

    public class TransactionDetailRequest : Request
    {
       // public int ID { get; set; }

        public string PaymentStatus { get; set; }
        public string TransactionID { get; set; }

        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

       


        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        public string sAction { get; set; }
    }

    public class PayPalTransactionRequest : Request
    {
       // public int ID { get; set; }

        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string OrderDateRange { get; set; }
    }

    public class PaypalApiRequest : Request
    {
        //public  int ID { get; set; }

        public string ApiUserName { get; set; }

        public bool IsActive { get; set; }
    }

    public class AssociationRequest : Request    
    {
        //public  int  ID { get; set; }

        public string ItemTitle { get; set; }
    }

    public class DispatchRequest : Request  
    {
        //之前没有继承
        //public int ID { get; set; }
        public string Agent { get; set; }
        public string Type { get; set; }
        public string Warehouse { get; set; }
        public string Express { get; set; }        
        public string ShiptoName { get; set; }       
        public string ShipToStreet { get; set; }        
        public string ShipToStreet2 { get; set; }       
        public string ShiptoCity { get; set; }     
        public string ShipToState { get; set; }       
        public string ShipToCountryName { get; set; }       
        public string ShipToZip { get; set; }
        public string goodsDescription { get; set; }
        public int goodsQuantity { get; set; }
        public decimal goodsDeclareWorth { get; set; }
        public string detailDescriptionCN { get; set; }
        public int goodsWeight { get; set; }
        public string size { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Custom { get; set; }
        public float InsuranceTip { get; set; }
    }

    public class AuditLogRequest : Request
    {
        public int ModelId { get; set; }
        public string UserName { get; set; }
        public string ModuleName { get; set; }
        public string TableName { get; set; }
    }
}
