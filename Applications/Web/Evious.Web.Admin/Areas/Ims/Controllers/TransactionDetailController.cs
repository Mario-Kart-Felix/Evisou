using Evious.Account.Contract;
using Evious.Framework.Utility;
using Evious.Ims.BLL;
using Evious.Ims.Contract.chukou1;
using Evious.Ims.Contract.Enum;
using Evious.Ims.Contract.Model;
using Evious.Web.Admin.Common;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.Admin.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_TransactionDetail)]
    public class TransactionDetailController : AdminControllerBase
    {
        #region Asynchronous processing in ASP.Net MVC with Ajax progress bar

        delegate string ProcessTask(string id, int PaypalApi, string RangeDate);
        LongRunningClass longRunningClass = new LongRunningClass();

        /// <summary>
        /// Starts the long running process.
        /// </summary>
        /// <param name="id">The id.</param>
        [HttpPost]
        [AllowAnonymous]
        public void StartLongRunningProcess(string id, int PaypalApi, string RangeDate)
        {
            longRunningClass.Add(id);            
            ProcessTask processTask = new ProcessTask(longRunningClass.ProcessLongRunningAction);
            processTask.BeginInvoke(id, PaypalApi, RangeDate, new AsyncCallback(EndLongRunningProcess), processTask);
        }
        /// <summary>
        /// Ends the long running process.
        /// </summary>
        /// <param name="result">The result.</param>
        public void EndLongRunningProcess(IAsyncResult result)
        {
            ProcessTask processTask = (ProcessTask)result.AsyncState;
            string id = processTask.EndInvoke(result);
            longRunningClass.Remove(id);
        }

        /// <summary>
        /// Gets the current progress.
        /// </summary>
        /// <param name="id">The id.</param>
        [AllowAnonymous]
        public ContentResult GetCurrentProgress(string id)
        {
            this.ControllerContext.HttpContext.Response.AddHeader("cache-control", "no-cache");
            var currentProgress = longRunningClass.GetStatus(id).ToString();
            return Content(currentProgress);
        }

        #endregion
        //
        // GET: /Ims/TransactionDetail/
        public ActionResult Index(TransactionDetailRequest request)
        {
            var result = this.ImsService.GetTransactionDetailList(request);

            ViewBag.PaypalApi = new SelectList(this.ImsService.GetPaypalApiList(), "ID", "PPAccount");            
            var expressAgent = EnumHelper.GetItemValueList<ExpressAgentEnum>();
            this.ViewBag.agent = new SelectList(expressAgent, "Key", "Value");
           /* List<int> aa=new List<int>();
            aa.Add(57);
            aa.Add(58);
            aa.Add(59);
            this.ImsService.DeletePaymentTransaction(aa);*/
            //ViewBag.Agent = new SelectList(, "ID", "PPAccount"); 
            //var aa = this.ImsService.GetPaymentTransaction(25);
            //Response.Write(aa.PaymentInfo.FeeAmount.value+"aa");
            return View(result);

          
        }

        public ActionResult GetExpressList(int agentid)
        {
            var result= this.ImsService.GetDriectExpress(agentid);
            return Json(result,
                             JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOutboundExpressList(int agentid,string warehouse)
        {
            var result = this.ImsService.GetOutboundExpress(agentid, warehouse);
            return Json(result,
                             JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sync()
        {
            ViewBag.PaypalApi = new SelectList(this.ImsService.GetPaypalApiList(), "ID", "PPAccount");          
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            return View();
        }

        public ActionResult Dispatch(int id)
        {
          //  var result = this.ImsService.GetTransactionDetailList(null);

            ViewBag.PaypalApi = new SelectList(this.ImsService.GetPaypalApiList(), "ID", "PPAccount");

            var expressAgent = EnumHelper.GetItemValueList<ExpressAgentEnum>();
            this.ViewBag.agent = new SelectList(expressAgent, "Key", "Value");

            var expressType = EnumHelper.GetItemValueList<ExpressTypeEnum>();
            this.ViewBag.type = new SelectList(expressType, "Key", "Value");


            var expressWarehouse = EnumHelper.GetItemList<EnumWarehouse>();//Enum.GetValues(typeof(EnumWarehouse)).Cast<EnumWarehouse>().Select(v => v.ToString()).ToList();
            this.ViewBag.warehouse = new SelectList(expressWarehouse, "Key", "Value");

            var model = this.ImsService.GetTransactionDetail(id);



            return View(model);
        }

        [HttpPost]
        public ActionResult Dispatch(FormCollection collection)
        {
            //int id =int.Parse(collection["ID"].ToString());
            //var model = this.ImsService.GetTransactionDetail(id);
          

            DispatchRequest tranasctiondetail=new DispatchRequest();  
            this.TryUpdateModel<DispatchRequest>(tranasctiondetail);
            var model = this.ImsService.GetTransactionDetail(tranasctiondetail.ID);
            tranasctiondetail.ShiptoName = model.ShiptoName;
            tranasctiondetail.ShipToStreet = model.ShipToStreet;
            tranasctiondetail.ShipToStreet2 = model.ShipToStreet2;
            tranasctiondetail.ShiptoCity = model.ShiptoCity;
            tranasctiondetail.ShipToState = model.ShipToState;
            tranasctiondetail.ShipToCountryName = model.ShipToCountryName;
            tranasctiondetail.ShipToZip = model.ShipToZip;
            ShipOrder order= this.ImsService.AddDispatchOrder(tranasctiondetail);

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDispatchOrder(int id)
        {
           string message= this.ImsService.DeleteDispatchOrder(id);
           return Json(new {message=message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitDispatchOrder(int id)
        {
            string message = this.ImsService.SubmitDispatchOrder(id);
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AjaxOrders(TransactionDetailRequest request)
        {
           // request.PaymentStatus = "COMPLETED";

            var alltransactions = this.ImsService.GetTransactionDetailList(null);
            IEnumerable<TransactionDetail> filterTransactions;

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                //Used if particulare columns are filtered 
                var receiverEmailFilter = Convert.ToString(Request["sSearch_1"]);
                var orderTimeFilter = Convert.ToString(Request["sSearch_2"]);
                var shipToCountryNameFilter = Convert.ToString(Request["sSearch_3"]);
                var buyerIDFilter = Convert.ToString(Request["sSearch_4"]);
                var transactionIDFilter = Convert.ToString(Request["sSearch_5"]);
                var trackingNumberFilter = Convert.ToString(Request["sSearch_6"]);
                var agentNameFilter = Convert.ToString(Request["sSearch_7"]);
                var shippingServiceFilter = Convert.ToString(Request["sSearch_8"]);
                //var orderStatusFilter = Convert.ToString(Request["sSearch_8"]);
                //var shippingFeeFilter = Convert.ToString(Request["sSearch_9"]);


                //Optionally check whether the columns are searchable at all 
                var isReceiverEmailSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                //var isOrderTimeable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isShipToCountryNameSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isBuyerIDSearchable = Convert.ToBoolean(Request["bSearchable_4"]);
                var isTransactionIDSearchable = Convert.ToBoolean(Request["bSearchable_5"]);
                var isTrackingNumberSearchable = Convert.ToBoolean(Request["bSearchable_6"]);
                var isAgentNameSearchable = Convert.ToBoolean(Request["bSearchable_7"]);
                var isShippingServiceSearchable = Convert.ToBoolean(Request["bSearchable_8"]);
                //var isOrderStatusSearchable = Convert.ToBoolean(Request["bSearchable_8"]);
                // var isShippingFeeSearchable = Convert.ToBoolean(Request["bSearchable_9"]);

                filterTransactions = alltransactions.Where(c => isReceiverEmailSearchable && c.ReceiverEmail.ToLower().Contains(request.sSearch.ToLower())
                    //|| isOrderTimeable && c.OrderTime.ToString().ToLower().Contains(request.sSearch.ToLower())
                    || isShipToCountryNameSearchable && c.ShipToCountryName.ToLower().Contains(request.sSearch.ToLower())
                    || isBuyerIDSearchable && c.BuyerID.ToLower().Contains(request.sSearch.ToLower())
                    || isTransactionIDSearchable && c.TransactionID.ToLower().Contains(request.sSearch.ToLower())
                    || isTrackingNumberSearchable && c.TrackingNumber.ToLower().Contains(request.sSearch.ToLower())
                    //|| isAgentNameSearchable && c.Agent.AgentName.ToLower().Contains(request.sSearch.ToLower())
                  //  || isShippingServiceSearchable && c.ShippingName.ToLower().Contains(request.sSearch.ToLower())
                    //|| isOrderStatusSearchable && c.OrderStatus.ToLower().Contains(request.sSearch.ToLower())      
                    //|| isShippingFeeSearchable && SqlFunctions.StringConvert(c.ActualShipAmount).Contains(request.sSearch.ToLower())
                    ).ToList();
            }
            else if (request.sAction == "filter")
            {
                var transactionIDFilter = Convert.ToString(Request["transaction_id"]).ToLower();
                var buyerIDFilter = Convert.ToString(Request["buyer_id"]).ToLower();
                var shiptocountrycodeFilter = Convert.ToString(Request["shiptocountrycode"]).ToLower().Trim();
                var ordertimerangeFilter = Convert.ToString(Request["ordertimerange"]).ToString();

                var isTransactionIDSearchable       = string.IsNullOrEmpty(Request["transaction_id"].ToString())? false : true;
                var isBuyerIDSearchable             = string.IsNullOrEmpty(Request["buyer_id"].ToString())?false:true;
                var isShipToCountryCodeSearchable   = string.IsNullOrEmpty(Request["shiptocountrycode"])?false:true;
                var isOrderTimeRangeSearchable      = string.IsNullOrEmpty(Request["ordertimerange"]) ? false : true;
                var isOrderPriceSearchable = string.IsNullOrEmpty(Request["order_base_price_from"]) && string.IsNullOrEmpty(Request["order_base_price_to"]) ? false : true;

                filterTransactions = this.ImsService.GetTransactionDetailList(null).Where(i=>i.ShipToCountryCode!=null);
                if (isTransactionIDSearchable)
                {
                    filterTransactions = filterTransactions.Where(c=>c.TransactionID.ToLower().Contains(transactionIDFilter));                
                }
                if (isBuyerIDSearchable)
                {
                    filterTransactions = filterTransactions.Where(c => c.BuyerID.ToLower().Contains(buyerIDFilter));         
                }
                if (isShipToCountryCodeSearchable)
                {
                    filterTransactions = filterTransactions.Where(c => c.ShipToCountryCode.ToLower().Contains(shiptocountrycodeFilter));
                }
                if (isOrderTimeRangeSearchable)
                {
                    string dataRange = ordertimerangeFilter;
                    string[] date = dataRange.Split(new char[] { '-' });

                    DateTime startDate = DateTime.Parse(date[0].Trim());
                    DateTime endDate = DateTime.Parse(date[1].Trim());
                    filterTransactions = filterTransactions.Where(c => c.OrderTime >= startDate).Where(c => c.OrderTime <=endDate);
                }
                if (isOrderPriceSearchable)
                {
                    filterTransactions = filterTransactions.Where(c => c.Amt >= decimal.Parse(Request["order_base_price_from"].ToString())).Where(c => c.Amt <= decimal.Parse(Request["order_base_price_to"].ToString()));
                }

            } else if (request.sAction == "filter_cancel")
            {
                filterTransactions = alltransactions;

            }
            else
            {
                filterTransactions = alltransactions;
            }
            
            var isOrderTimeSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isTransactionIDSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isBuyerIDSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isShipToCountryCodeSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isPriceSortable = Convert.ToBoolean(Request["bSortable_5"]);
            var isTrackingNumberSortable = Convert.ToBoolean(Request["bSortable_6"]);
            var isAgentNameSortable = Convert.ToBoolean(Request["bSortable_7"]);
            var isShippingServiceSortable = Convert.ToBoolean(Request["bSortable_8"]);
            // var isOrderStatusSortable = Convert.ToBoolean(Request["bSortable_8"]);
            //var isShippingFeeSortable = Convert.ToBoolean(Request["bSortable_9"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            Func<TransactionDetail, string> orderingFunction = (c => 
                                                            sortColumnIndex ==1 && isOrderTimeSortable ? c.OrderTime.ToString() : 
                                                            sortColumnIndex == 2 && isTransactionIDSortable ? c.TransactionID :                                                                                                                   
                                                            sortColumnIndex == 3 && isBuyerIDSortable ? c.BuyerID :
                                                            sortColumnIndex == 4 && isShipToCountryCodeSortable ? c.ShipToCountryCode :
                                                            sortColumnIndex == 5 && isPriceSortable ? c.Amt.ToString() :
                                                          //sortColumnIndex == 5 && isTrackingNumberSortable ? c.TrackingNumber :
                                                         // sortColumnIndex == 6 && isAgentNameSortable ? (c.Agent == null ? "" : c.Agent.AgentName) :
                                                        //  sortColumnIndex == 7 && isShippingServiceSortable ? c.ShippingName :
                //sortColumnIndex == 8 && isOrderStatusSortable ? c.OrderStatus :
                //sortColumnIndex == 9 && isShippingFeeSortable ? c.ActualShipAmount.ToString() :
                                                          "");

            filterTransactions = filterTransactions.OrderByDescending(i => i.OrderTime);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
            {
                filterTransactions = filterTransactions.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterTransactions = filterTransactions.OrderByDescending(orderingFunction);
            }

            var displayedTransaction = filterTransactions.Skip(request.iDisplayStart).Take(request.iDisplayLength);

            var result = from c in displayedTransaction
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.OrderTime.ToString(@"yyyy-MM-dd")
                                            , c.TransactionID
                                            , c.BuyerID
                                            , c.ShipToCountryCode
                                            , c.Amt.ToString()
                                            , c.Agent.ToString()
                                            , c.Express//c.AgentPostage.ToString("#0.00")//"<span class=\"label label-sm label-danger\">On Hold</span>"
                                            //, "<a href=\"ecommerce_orders_view.html\" class=\"btn btn-xs default btn-editable\"><i class=\"fa fa-search\"></i> View</a>"
                                            ,Convert.ToString(c.ID)
                         
                         };

            return Json(new
            {
                sEcho = request.sEcho,//param.sEcho,
                iTotalRecords = alltransactions.Count(),//alltransactions.Count(),
                iTotalDisplayRecords = filterTransactions.Count(),
                aaData = result
            },
                             JsonRequestBehavior.AllowGet);
        }
	}
}