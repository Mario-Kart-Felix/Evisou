using Evisou.Account.Contract;
using Evisou.Framework.Utility;
using Evisou.Framework.Web;
using Evisou.Ims.Contract.chukou1;
using Evisou.Ims.Contract.Enum;
using Evisou.Ims.Contract.Model;
using Evisou.Ims.Contract.Model.PayPal;
using Evisou.Web.AdminApplication.Common;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Ims.Controllers
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
           //string x=await longRunningClass.PayPalTransactionProcessLongRunningAction2(id, PaypalApi, RangeDate);
            ProcessTask processTask = new ProcessTask(longRunningClass.PayPalTransactionProcessLongRunningAction);
           //ProcessTask processTask = new ProcessTask(x);
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
        public ActionResult GetCurrentProgress(string id)
        {
            this.ControllerContext.HttpContext.Response.AddHeader("cache-control", "no-cache");
            var currentProgress = longRunningClass.GetStatusDic(id);
            return Json(new
            {

                data = currentProgress
            },
                             JsonRequestBehavior.AllowGet);

         //   return Content(currentProgress);
        }

        #endregion

        //public async Task<ActionResult> Datatable(PayPalTransactionRequest request)
        public ActionResult Datatable3(PayPalTransactionRequest request)
        {


            #region 自定义动作
            //if (!string.IsNullOrEmpty(request.customActionType))
            //{
            //    string[] id_Array = Request.Params.GetValues("id[]");
            //    List<int> ids = new List<int>();

            //    foreach (string i in id_Array)
            //    {
            //        ids.Add(int.Parse(i));
            //    }

            //    switch (request.customActionType)
            //    {
            //        case "group_action":

            //            switch (request.customActionName)
            //            {
            //                case "delete":
            //                    // DeleteImageFile(id_Array);
            //                    DeleteImageFileIO(ids);
            //                    this.ImsService.DeleteProduct(ids);
            //                    break;

            //            }
            //            break;

            //        case "delete":
            //            // DeleteImageFile(id_Array);
            //            DeleteImageFileIO(ids);
            //            this.ImsService.DeleteProduct(ids);
            //            break;
            //    }
            //}
            #endregion

           
           // var allTransaction = await this.ImsService.GetPayPalTransactionListAsync(null);
            var allTransaction =  this.ImsService.GetPayPalTransactionList(null);
            IEnumerable<PayPalTransaction> filterTransaction = allTransaction;

            #region 搜索


            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var OrderTimeFromFilter = Convert.ToString(Request["ordertime_from"]).Trim().ToLower();
                var OrderTimeToFilter = Convert.ToString(Request["ordertime_to"]).Trim().ToLower();

               // var SkuFilter = Convert.ToString(Request["sku"]).Trim().ToLower();

                //var PackingWeightFilter = Convert.ToString(Request["packingweight"]).Trim().ToLower();

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterTransaction = filterTransaction.
                        Where(c => c.PaymentDate <= DateTime.Parse(OrderTimeToFilter) 
                            && c.PaymentDate >= DateTime.Parse(OrderTimeFromFilter)
                            );
                }

                //var isSkuSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());


                //if (isSkuSearchable)
                //{
                //    filterProduct = filterProduct.Where(c => c.Sku.ToLower().Contains(SkuFilter));
                //}

                //var isPackingWeightSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                //if (isPackingWeightSearchable && !string.IsNullOrEmpty(PackingWeightFilter))
                //{
                //    filterProduct = filterProduct.Where(c => c.PackWeight == PackingWeightFilter.ToInt());
                //}

            }
            else if (request.action == "filter_cancel")
            {
                filterTransaction = allTransaction;
            }
            else
            {
                filterTransaction = allTransaction;
            }
            #endregion

            #region 排序
            var isOrderTimeSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isTransactionIDSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            //var isBuyerIDSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
           

            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<PayPalTransaction, string> orderingFunction = (c =>
                                               sortColumnIndex == 1 && isOrderTimeSortable ? DateTime.Parse(c.PaymentDate.ToString()).ToCnDataString() :
                                               sortColumnIndex == 2 && isTransactionIDSortable ? c.TransactionId :
                                               
                                                "");

            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterTransaction = filterTransaction.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterTransaction = filterTransaction.OrderByDescending(orderingFunction);
            }

            #endregion

            
            var displayedTransaction = filterTransaction.Skip(request.start).Take(request.length);
            var result = from c in displayedTransaction
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            ,DateTime.Parse(c.PaymentDate.ToString()).ToCnDataString()//c.OrderTime.ToCnDataString()
                                            , c.TransactionId//c.TransactionID
                                            , c.BuyerID//c.BuyerID
                                            , c.PayerCountryName//c.ShipToCountryCode
                                            , c.GrossAmount.ToPrice()+" "+c.CurrencyCode
                                            , c.Agent.ToString()
                                            ,c.Express                                     
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allTransaction.Count(),//alltransactions.Count(),
                recordsFiltered = filterTransaction.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        public ActionResult Datatable(PayPalTransactionRequest request)
       // public async Task<ActionResult> Datatable(PayPalTransactionRequest request)
        {
            #region 自定义动作
            //if (!string.IsNullOrEmpty(request.customActionType))
            //{
            //    string[] id_Array = Request.Params.GetValues("id[]");
            //    List<int> ids = new List<int>();

            //    foreach (string i in id_Array)
            //    {
            //        ids.Add(int.Parse(i));
            //    }

            //    switch (request.customActionType)
            //    {
            //        case "group_action":

            //            switch (request.customActionName)
            //            {
            //                case "delete":
            //                    // DeleteImageFile(id_Array);
            //                    DeleteImageFileIO(ids);
            //                    this.ImsService.DeleteProduct(ids);
            //                    break;

            //            }
            //            break;

            //        case "delete":
            //            // DeleteImageFile(id_Array);
            //            DeleteImageFileIO(ids);
            //            this.ImsService.DeleteProduct(ids);
            //            break;
            //    }
            //}
            #endregion            
            var allTransaction = this.ImsService.GetPayPalTransactionList(null);
           // var allTransaction = await this.ImsService.GetPayPalTransactionListAsync2(null);
           
            IEnumerable<PayPalTransaction> filterTransaction = allTransaction;
            
            #region 搜索

           switch (request.action)
           {
               case "filter":
                   Func<PayPalTransaction, bool> Filter = (
                               c => {
                                   bool flag=true;
                                   if (Convert.ToBoolean(Request["columns[1][searchable]"].ToString())&& !string.IsNullOrEmpty(request.OrderDateRange))
                                   {                                       
                                       flag= flag&& c.PaymentDate <= DateTime.Parse(request.OrderDateRange.Split('-')[1])
                                       && c.PaymentDate >= DateTime.Parse(request.OrderDateRange.Split('-')[0]);
                                   }
                                   if (Convert.ToBoolean(Request["columns[2][searchable]"].ToString()) && !string.IsNullOrEmpty(request.TransactionId))
                                   {
                                       flag =flag && c.TransactionId.Contains(request.TransactionId.Trim().ToUpper());
                                   }
                                   return flag;                            
                               }
                       );
                   filterTransaction = filterTransaction.Where(Filter);
                   break;
               case "filter_cancel":
                   filterTransaction = allTransaction;
                   break;

               default:
                   filterTransaction = allTransaction;
                   break;
           }

            #endregion
            #region 排序
            var isOrderTimeSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isTransactionIDSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);           


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<PayPalTransaction, string> orderingFunction = ( c=>
                                               sortColumnIndex == 1 && isOrderTimeSortable ? DateTime.Parse(c.PaymentDate.ToString()).ToCnDataString() :
                                               sortColumnIndex == 2 && isTransactionIDSortable ? c.TransactionId :

                                                "");

            var sortDirection = Request["order[0][dir]"];

            switch (sortDirection)
            {
                case "asc":
                    filterTransaction = filterTransaction.OrderBy(orderingFunction);
                    break;
                case "desc":
                    filterTransaction = filterTransaction.OrderByDescending(orderingFunction);
                    break;
                default:
                    filterTransaction = filterTransaction.OrderBy(orderingFunction);
                    break;
            }

            
            #endregion
            var displayedTransaction = filterTransaction.Skip(request.start).Take(request.length);
            var result = from c in displayedTransaction
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            ,DateTime.Parse(c.PaymentDate.ToString()).ToCnDataString()
                                            , c.TransactionId
                                            , c.BuyerID
                                            , c.PayerCountryName
                                            , c.GrossAmount.ToPrice()+" "+c.CurrencyCode
                                            , c.Agent.ToString()
                                            ,c.Express                                     
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,
                recordsTotal = allTransaction.Count(),
                recordsFiltered = filterTransaction.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(TransactionDetailRequest request)
        {
            var result = this.ImsService.GetTransactionDetailList(request);

            ViewBag.PaypalApi = new SelectList(this.ImsService.GetPaypalApiList(), "ID", "PPAccount");
            var expressAgent = EnumHelper.GetItemValueList<ExpressAgentEnum>();
            this.ViewBag.agent = new SelectList(expressAgent, "Key", "Value");           
            return View(result);


        }

        public ActionResult GetExpressList(int agentid)
        {
            var result = this.ImsService.GetDriectExpress(agentid);
            return Json(result,
                             JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOutboundExpressList(int agentid, string warehouse)
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
           // var result = this.ImsService.GetTransactionDetailList(null);

            ViewBag.PaypalApi = new SelectList(this.ImsService.GetPaypalApiList(), "ID", "PPAccount");

            var expressAgent = EnumHelper.GetItemValueList<ExpressAgentEnum>().OrderBy(i=>i.Value);
            this.ViewBag.agent = new SelectList(expressAgent, "Key", "Value");
            this.ViewBag.agents = expressAgent;


            var expressType = EnumHelper.GetItemValueList<ExpressTypeEnum>();
            this.ViewBag.type = new SelectList(expressType, "Key", "Value");


            var expressWarehouse = EnumHelper.GetItemList<EnumWarehouse>();//Enum.GetValues(typeof(EnumWarehouse)).Cast<EnumWarehouse>().Select(v => v.ToString()).ToList();
            this.ViewBag.warehouse = new SelectList(expressWarehouse, "Key", "Value");

            var model = this.ImsService.GetPayPalTransaction(id);



            return View(model);
           // return View();
        }

        [HttpPost]
        public ActionResult Dispatch(DispatchRequest tranasctiondetail)
        {
            var model = this.ImsService.GetPayPalTransaction(tranasctiondetail.ID);
            tranasctiondetail.ShiptoName = model.PayerAddressName;
            tranasctiondetail.ShipToStreet = model.PayerAddressStreet1;
            tranasctiondetail.ShipToStreet2 = model.PayerAddressStreet2;
            tranasctiondetail.ShiptoCity = model.PayerCityName;
            tranasctiondetail.ShipToState = model.PayerStateOrProvince;
            tranasctiondetail.ShipToCountryName = model.PayerCountryName;
            tranasctiondetail.ShipToZip = model.PayerPostalCode;
            ShipOrder order = this.ImsService.AddDispatchOrder(tranasctiondetail, model);

            return Json(order, JsonRequestBehavior.AllowGet);

          
        }

        [HttpPost]
        public ActionResult DeleteDispatchOrder(int id)
        {
            string message = this.ImsService.DeleteDispatchOrder(id);
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitDispatchOrder(int id)
        {
            var model = this.ImsService.GetPaymentTransaction(id);
            string message = this.ImsService.SubmitDispatchOrder(model);
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

                var isTransactionIDSearchable = string.IsNullOrEmpty(Request["transaction_id"].ToString()) ? false : true;
                var isBuyerIDSearchable = string.IsNullOrEmpty(Request["buyer_id"].ToString()) ? false : true;
                var isShipToCountryCodeSearchable = string.IsNullOrEmpty(Request["shiptocountrycode"]) ? false : true;
                var isOrderTimeRangeSearchable = string.IsNullOrEmpty(Request["ordertimerange"]) ? false : true;
                var isOrderPriceSearchable = string.IsNullOrEmpty(Request["order_base_price_from"]) && string.IsNullOrEmpty(Request["order_base_price_to"]) ? false : true;

                filterTransactions = this.ImsService.GetTransactionDetailList(null).Where(i => i.ShipToCountryCode != null);
                if (isTransactionIDSearchable)
                {
                    filterTransactions = filterTransactions.Where(c => c.TransactionID.ToLower().Contains(transactionIDFilter));
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
                    filterTransactions = filterTransactions.Where(c => c.OrderTime >= startDate).Where(c => c.OrderTime <= endDate);
                }
                if (isOrderPriceSearchable)
                {
                    filterTransactions = filterTransactions.Where(c => c.Amt >= decimal.Parse(Request["order_base_price_from"].ToString())).Where(c => c.Amt <= decimal.Parse(Request["order_base_price_to"].ToString()));
                }

            }
            else if (request.sAction == "filter_cancel")
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
                                                            sortColumnIndex == 1 && isOrderTimeSortable ? c.OrderTime.ToString() :
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

        [HttpPost]
        public ActionResult Stats(string rangedate)
        {
            DateTime start = DateTime.Parse(rangedate.Split('-')[0]);
            DateTime end = DateTime.Parse(rangedate.Split('-')[1]);
            string year = Request["year"] ?? "2014";
            //string daysaleinmonth = Request["daysaleinmonth"] ?? "2014";
            var transactions = this.ImsService.GetPayPalTransactionList(null);
           
            var calendar = from c in transactions
                          // where c.PaymentDate<=end&&c.PaymentDate>=start
                      group new
                      {
                          start =c.PaymentDate.ToCnDataString(),
                          end = c.PaymentDate.ToCnDataString(),
                      }
                      by c.PaymentDate.ToCnDataString()
                          into ItemGp
                          select new { 
                            start=ItemGp.Key,
                            end=ItemGp.Key,
                            title= ItemGp.Count(),
                            url="http://www.baidu.com"
                          };

            var monthsale = from c in transactions                            
                            where c.PaymentDate<=end
                            &&c.PaymentDate>=start
                            &&c.CurrencyCode == CurrencyCodeType.GBP.ToString()
                            orderby c.PaymentDate ascending
                           group c by c.PaymentDate.ToString("yyyy-MM")                           
                           into ItemGp 
                               select new []
                               {
                                  DateTime.Parse(ItemGp.Key).ToString("yy年MMM"),
                                  ItemGp.Sum(c=>c.GrossAmount).ToString(),
                                
                               };
            var monthsaleamount = (from c in transactions
                                 where
                                 c.PaymentDate<=end
                                    &&c.PaymentDate>=start
                                    &&c.CurrencyCode == CurrencyCodeType.GBP.ToString()
                                 select c.GrossAmount
                                     ).Sum();
            var monthsaleqty = (from c in transactions
                               where
                              c.PaymentDate <= end
                            && c.PaymentDate >= start
                            && c.CurrencyCode == CurrencyCodeType.GBP.ToString()
                               select c).Count();




            string daysaleinmonthend = Request["daysaleinmonth"] != null ? "2011-05-31" : "2011-05-08";

            var daysale = from c in transactions
                            where
                            c.PaymentDate >= DateTime.Parse("2011-05-01") &&
                            c.PaymentDate <= DateTime.Parse(daysaleinmonthend) &&
                             c.CurrencyCode == CurrencyCodeType.GBP.ToString()
                            orderby c.PaymentDate ascending
                            group c by c.PaymentDate.ToString("yyyy-MM-dd")
                                into ItemGp
                                select new[]
                               {
                                 Request["daysaleinmonth"] != null? DateTime.Parse(ItemGp.Key).ToString("dd"):DateTime.Parse(ItemGp.Key).ToString("ddd(MMMd日)"),
                                  ItemGp.Sum(c=>c.GrossAmount).ToString(),
                                
                               };

           var totalamount = (from c in transactions
                              where c.CurrencyCode == CurrencyCodeType.GBP.ToString()
                           select c.GrossAmount
                                     ).Sum();

            var totalqty= (from c in transactions
                               where
                                c.CurrencyCode == CurrencyCodeType.GBP.ToString()
                               select c).Count();

            //Func<Evisou.Core.Log.AuditLog, string> content2 = (c =>c.ModuleName);


            //var auditloglist = this.ImsService.GetAuditLogList(null);
            //var auditlogs = (from c in auditloglist
            //                 where c.UserName == Evisou.Web.AdminApplication.Common.AdminUserContext.Current.LoginInfo.LoginName
            //                 orderby c.CreateTime descending
            //                 select new
            //                 {
            //                     createtime = c.CreateTime.ToRecentDay(),
            //                     content = //"用户:" + c.UserName + " " + 
            //                     c.EventType + " " + "模块:" 
            //                     + c.ModuleName + " " + "值为:" 
            //                     + StringUtil.CutString(StringUtil.RemoveHtml(c.NewValues), 40),
            //                 }
            //                    ).Take(20); 

                

            return Json(new
            {
                data = new
                {
                    calresult = calendar,
                    monthsale = new
                    {
                        monthsaleresult = monthsale,
                        monthsaleamount = monthsaleamount,
                        monthsaleqty = monthsaleqty
                    },
                    daysale = new
                    {
                        daysaleresult = daysale
                    },
                    total = new {
                        amount = totalamount,
                        qty = totalqty
                    },                    
                    logs=new{
                       auditlogs=new object()
                    }
                }
            },
                             JsonRequestBehavior.AllowGet);

        }

        public ActionResult Stats2()
        {
            string year = Request["year"] ?? "2014";
            //string daysaleinmonth = Request["daysaleinmonth"] ?? "2014";
            var transactions = this.ImsService.GetPaymentTransactionList(null);

            var calendar = from c in transactions
                           group new
                           {
                               start = DateTime.Parse(c.PaymentInfo.PaymentDate).ToCnDataString(),
                               end = DateTime.Parse(c.PaymentInfo.PaymentDate).ToCnDataString(),
                           }
                           by DateTime.Parse(c.PaymentInfo.PaymentDate).ToCnDataString()
                               into ItemGp
                               select new
                               {
                                   start = ItemGp.Key,
                                   end = ItemGp.Key,
                                   title = ItemGp.Count(),
                                   url = "http://www.baidu.com"
                               };

            var monthsale = from c in transactions
                            where
                            DateTime.Parse(c.PaymentInfo.PaymentDate).ToString("yyyy") == year &&
                            c.PaymentInfo.GrossAmount.currencyID == CurrencyCodeType.GBP
                            orderby c.PaymentInfo.PaymentDate ascending
                            group c by DateTime.Parse(c.PaymentInfo.PaymentDate).ToString("yyyy-MM")
                                into ItemGp
                                select new[]
                               {
                                  DateTime.Parse(ItemGp.Key).ToString("MMM"),
                                  ItemGp.Sum(c=>c.PaymentInfo.GrossAmount.value.ToDecimal()).ToString(),
                                
                               };
            var monthsaleamount = (from c in transactions
                                   where
                                   DateTime.Parse(c.PaymentInfo.PaymentDate).ToString("yyyy") == year &&
                                   c.PaymentInfo.GrossAmount.currencyID == CurrencyCodeType.GBP
                                   select c.PaymentInfo.GrossAmount.value.ToDecimal()
                                     ).Sum();
            var monthsaleqty = (from c in transactions
                                where
                                DateTime.Parse(c.PaymentInfo.PaymentDate).ToString("yyyy") == year &&
                                c.PaymentInfo.GrossAmount.currencyID == CurrencyCodeType.GBP
                                select c).Count();




            string daysaleinmonthend = Request["daysaleinmonth"] != null ? "2011-05-31" : "2011-05-08";

            var daysale = from c in transactions
                          where
                          DateTime.Parse(c.PaymentInfo.PaymentDate) >= DateTime.Parse("2011-05-01") &&
                          DateTime.Parse(c.PaymentInfo.PaymentDate) <= DateTime.Parse(daysaleinmonthend) &&
                          c.PaymentInfo.GrossAmount.currencyID == CurrencyCodeType.GBP
                          orderby c.PaymentInfo.PaymentDate ascending
                          group c by DateTime.Parse(c.PaymentInfo.PaymentDate).ToString("yyyy-MM-dd")
                              into ItemGp
                              select new[]
                               {
                                 Request["daysaleinmonth"] != null? DateTime.Parse(ItemGp.Key).ToString("dd"):DateTime.Parse(ItemGp.Key).ToString("ddd(MMMd日)"),
                                  ItemGp.Sum(c=>c.PaymentInfo.GrossAmount.value.ToDecimal()).ToString(),
                                
                               };

            var totalamount = (from c in transactions
                               where c.PaymentInfo.GrossAmount.currencyID == CurrencyCodeType.GBP
                               select c.PaymentInfo.GrossAmount.value.ToDecimal()
                                      ).Sum();

            var totalqty = (from c in transactions
                            where
                            c.PaymentInfo.GrossAmount.currencyID == CurrencyCodeType.GBP
                            select c).Count();

            //Func<Evisou.Core.Log.AuditLog, string> content2 = (c => c.ModuleName);


            //var auditloglist = this.ImsService.GetAuditLogList(null);
            //var auditlogs = (from c in auditloglist
            //                 where c.UserName == Evisou.Web.AdminApplication.Common.AdminUserContext.Current.LoginInfo.LoginName
            //                 orderby c.CreateTime descending
            //                 select new
            //                 {
            //                     createtime = c.CreateTime.ToRecentDay(),
            //                     content = //"用户:" + c.UserName + " " + 
            //                     c.EventType + " " + "模块:"
            //                     + c.ModuleName + " " + "值为:"
            //                     + StringUtil.CutString(StringUtil.RemoveHtml(c.NewValues), 40),
            //                 }
            //                    ).Take(20);

            //var auditlogs = string.Empty;     

            return Json(new
            {
                data = new
                {
                    calresult = calendar,
                    monthsale = new
                    {
                        monthsaleresult = monthsale,
                        monthsaleamount = monthsaleamount,
                        monthsaleqty = monthsaleqty
                    },
                    daysale = new
                    {
                        daysaleresult = daysale
                    },
                    total = new
                    {
                        amount = totalamount,
                        qty = totalqty
                    },
                    //cpu = temp//HR.GetCpuInfo()
                    logs = new
                    {
                        auditlogs = new object()
                    }
                }
            },
                             JsonRequestBehavior.AllowGet);

        }

        public ActionResult ServerStats() 
        {

            string memory = ((Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2");// "M";
             string cpu = ((TimeSpan)System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime).TotalSeconds.ToString("N0");

            return Json(new
            {
                data = new
                {   cpu=cpu,
                    memory=memory
                }
            },JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentItemList(int id)
        {
            var model = this.ImsService.GetPayPalTransaction(id);

           // var addresses = db.Addresses.Where(a => a.PersonID == id).OrderBy(a => a.City);

            return PartialView("PaymentItemList", model);
        }

        public void DataExport(string ids,string type)
        {
            string[] idArray = Uri.EscapeUriString(ids).Split(',');
            List<int> IDSList = new List<int>();
            foreach (string i in idArray)
            {
                IDSList.Add(i.ToInt());
            }
            this.ImsService.PayPalTransactionDataExport(IDSList, type);
          
        }

        
    }
}