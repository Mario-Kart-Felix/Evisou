using Evious.Ims.Contract.Model;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading;
using Evious.Ims.DAL;
using System.Web.Mvc;
using Evious.Framework.Utility;
using System.Threading.Tasks;

namespace Evious.Web
{
    public class LongRunningClass : ControllerBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LongRunningClass"/> class.
        /// </summary>
        public LongRunningClass()
        {
            if (ProcessStatus == null)
            {
                ProcessStatus = new Dictionary<string, int>();
            }
        }

       private static object syncRoot = new object();
    
        /// <summary>
        /// 获取或者设置处理状态/ Gets or sets the process status.
        /// </summary>
        /// <value>处理状态/The process status.</value>
        private static IDictionary<string, int> ProcessStatus { get; set; }

        private static string ProcessLastDate { get; set; }
        private static int ProcessTotal { get; set; }
        private static int ProcessNumber { get; set; }
        private static int Precentage(int x, int y)
        {
            int a = x + 1;
            decimal b = Math.Round((decimal)a / y, 2);
            int c = (int)(b * 100);
            return c;
        }

        //int total{ get; set; }
        /// <summary>
        /// 处理长运行动作/Processes the long running action.
        /// </summary>
        /// <param name="id">动作id/The id.</param>        ///
        public string ProcessLongRunningAction(string id, int PaypalApi, string RangeDate)
        {
            PaypalApi paypalApi = this.ImsService.GetPaypalApi(PaypalApi);

            this.ImsService.PayPalHelper(paypalApi);

            string dataRange = RangeDate;
            string[] date = dataRange.Split(new char[] { '-' });

            DateTime startDate = DateTime.Parse(date[0].Trim());
            DateTime endDate = DateTime.Parse(date[1].Trim());

            IEnumerable<PaymentTransactionSearchResultType> PaypalTransactionSearch = this.ImsService.ApiTransactionSearch(startDate, endDate);
            var total = PaypalTransactionSearch.Count<PaymentTransactionSearchResultType>();


            int i = 0;
            foreach (var item in PaypalTransactionSearch)
            {

                GetTransactionDetailsResponseType PaypalTransactionDetails = this.ImsService.ApiTransactionDetail(item);
                if (PaypalTransactionDetails.Ack.Equals(AckCodeType.SUCCESS))
                {                    
                    i++;
                    int d = Precentage(i, total);

                    lock (syncRoot)
                    {
                        ProcessStatus[id] = d;
                    }
                    TransactionDetailRequest request = new TransactionDetailRequest
                    {
                        TransactionID = item.TransactionID
                    };
                   if(this.ImsService.GetTransactionDetailList(request).Count()==0)
                   {
                       var detail = this.ImsService.TransactionDetail(PaypalTransactionDetails);
                       using (var dbContext = new ImsDbContext())
                       {                           
                           dbContext.Insert<TransactionDetail>(detail);
                       }
                   }
                    
                }
            }                 

            return id;
        }


        public string PayPalTransactionProcessLongRunningAction(string id, int PaypalApi, string RangeDate)
        {
            using (ImsDbContext dbContext = new ImsDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                PaypalApi paypalApi = dbContext.PaypalApis.Find(PaypalApi);
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                this.ImsService.PayPalHelper(paypalApi);               
                string dataRange = RangeDate;
                string[] date = dataRange.Split(new char[] { '-' });

                DateTime startDate = DateTime.Parse(date[0].Trim());
                DateTime endDate = DateTime.Parse(date[1].Trim());

                IEnumerable<PaymentTransactionSearchResultType> PaypalTransactionSearch = this.ImsService.ApiTransactionSearch(startDate, endDate);
                int total = PaypalTransactionSearch.Count<PaymentTransactionSearchResultType>();


                if (PaypalTransactionSearch.Count() > 0)
                {
                    var lastsearch = PaypalTransactionSearch.Last();
                    var lastdetail = this.ImsService.ApiTransactionDetail(lastsearch);
                    ProcessLastDate = lastdetail.PaymentTransactionDetails.PaymentInfo.PaymentDate;
                }
                else
                {
                    ProcessLastDate = startDate.ToCnDataString();
                }
                int i = 0;
               // List<PayPalTransaction> pptlist = new List<PayPalTransaction>();
                PaypalTransactionSearch.ToList().ForEach(a =>
                {
                    GetTransactionDetailsResponseType PaypalTransactionDetails = this.ImsService.ApiTransactionDetail(a);
                    if (PaypalTransactionDetails.Ack.Equals(AckCodeType.SUCCESS))
                    {
                        i++;
                        int d = Precentage(i, total);

                        lock (syncRoot)
                        {
                            ProcessStatus[id] = d;
                            ProcessTotal = total;
                            ProcessNumber = i + 1;
                        }
                        dbContext.Configuration.AutoDetectChangesEnabled = false;
                        bool flag = dbContext.PayPalTransactions.AsNoTracking().Count(j => j.TransactionId == a.TransactionID) == 0;
                        dbContext.Configuration.AutoDetectChangesEnabled = true;
                        if (flag)
                        {
                            var detail = this.ImsService.PayPayTransaction(PaypalTransactionDetails);
                           // pptlist.Add(detail);
                            dbContext.Insert<PayPalTransaction>(detail);
                        }
                    }
                });
               // dbContext.PayPalTransactions.AddRange(pptlist);
               // dbContext.SaveChanges();
            }
            return id;
        }

        public async Task<string> PayPalTransactionProcessLongRunningAction2(string id, int PaypalApi, string RangeDate)
        {
            using (ImsDbContext dbContext = new ImsDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                PaypalApi paypalApi =await dbContext.PaypalApis.FindAsync(PaypalApi);
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                this.ImsService.PayPalHelper(paypalApi);
                string dataRange = RangeDate;
                string[] date = dataRange.Split(new char[] { '-' });

                DateTime startDate = DateTime.Parse(date[0].Trim());
                DateTime endDate = DateTime.Parse(date[1].Trim());

                IEnumerable<PaymentTransactionSearchResultType> PaypalTransactionSearch = this.ImsService.ApiTransactionSearch(startDate, endDate);
                int total = PaypalTransactionSearch.Count<PaymentTransactionSearchResultType>();


                if (PaypalTransactionSearch.Count() > 0)
                {
                    var lastsearch = PaypalTransactionSearch.Last();
                    var lastdetail = this.ImsService.ApiTransactionDetail(lastsearch);
                    ProcessLastDate = lastdetail.PaymentTransactionDetails.PaymentInfo.PaymentDate;
                }
                else
                {
                    ProcessLastDate = startDate.ToCnDataString();
                }
                int i = 0;
                List<PayPalTransaction> pptlist = new List<PayPalTransaction>();
                PaypalTransactionSearch.ToList().ForEach(a =>
                {
                    GetTransactionDetailsResponseType PaypalTransactionDetails = this.ImsService.ApiTransactionDetail(a);
                    if (PaypalTransactionDetails.Ack.Equals(AckCodeType.SUCCESS))
                    {
                        i++;
                        int d = Precentage(i, total);

                        lock (syncRoot)
                        {
                            ProcessStatus[id] = d;
                            ProcessTotal = total;
                            ProcessNumber = i + 1;
                        }
                      
                        dbContext.Configuration.AutoDetectChangesEnabled = false;                      
                        bool flag = dbContext.PayPalTransactions.AsNoTracking().Count(j => j.TransactionId == a.TransactionID) == 0;
                        dbContext.Configuration.AutoDetectChangesEnabled = true;
                        if (flag)
                        {
                            var detail = this.ImsService.PayPayTransaction(PaypalTransactionDetails);
                            pptlist.Add(detail);
                        }
                    }
                });
                dbContext.PayPalTransactions.AddRange(pptlist);
                await dbContext.SaveChangesAsync();               
            }
            return id;
        }
        public string PaymentTransactionProcessLongRunningAction(string id, int PaypalApi, string RangeDate)
        {
            PaypalApi paypalApi = this.ImsService.GetPaypalApi(PaypalApi);

            this.ImsService.PayPalHelper(paypalApi);

            string dataRange = RangeDate;
            string[] date = dataRange.Split(new char[] { '-' });

            DateTime startDate = DateTime.Parse(date[0].Trim());
            DateTime endDate = DateTime.Parse(date[1].Trim());

            IEnumerable<PaymentTransactionSearchResultType> PaypalTransactionSearch = this.ImsService.ApiTransactionSearch(startDate, endDate);
            var total = PaypalTransactionSearch.Count<PaymentTransactionSearchResultType>();


            int i = 0;
            foreach (var item in PaypalTransactionSearch)
            {

                GetTransactionDetailsResponseType PaypalTransactionDetails = this.ImsService.ApiTransactionDetail(item);
                if (PaypalTransactionDetails.Ack.Equals(AckCodeType.SUCCESS))
                {

                    i++;
                    int d = Precentage(i, total);

                    lock (syncRoot)
                    {
                        ProcessStatus[id] = d;
                    }


                    /*TransactionDetailRequest request = new TransactionDetailRequest
                    {
                        TransactionID = item.TransactionID
                    };*/

                    PaymentTransactionRequest request = new PaymentTransactionRequest
                    {
                        TransactionID = item.TransactionID
                    };
                    if (this.ImsService.GetPaymentTransactionList(request).Count() == 0)
                    {
                        
                        var detail = this.ImsService.PaymentTransaction(PaypalTransactionDetails);

                        using (var dbContext = new ImsDbContext())
                        {

                            dbContext.Insert<Evious.Ims.Contract.Model.PayPal.PaymentTransactionType>(detail);


                        }
                    }

                }
            }

            return id;
        }

        /// <summary>
        /// 添加指定的ID/Adds the specified id.
        /// </summary>
        /// <param name="id">动作id/The id.</param>
        public void Add(string id)
        {
            lock (syncRoot)
            {
                ProcessStatus.Add(id, 0);
            }
        }

        /// <summary>
        /// 移除指定的ID/Removes the specified id.
        /// </summary>
        /// <param name="id">动作id/The id.</param>
        public void Remove(string id)
        {
            lock (syncRoot)
            {
                ProcessStatus.Remove(id);
            }
        }

        /// <summary>
        /// 获取状态/Gets the status.
        /// </summary>
        /// <param name="id">动作id/The id.</param>
        public int GetStatus(string id)
        {
            lock (syncRoot)
            {
                if (ProcessStatus.Keys.Count(x => x == id) == 1)
                {
                    return ProcessStatus[id];
                }
                else
                {
                    return 100;
                }
            }
        }

        public Dictionary<string, object> GetStatusDic(string id)
        {

            lock (syncRoot)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (ProcessStatus.Keys.Count(x => x == id) == 1)
                {                   
                    dic.Add("percentage", ProcessStatus[id]);
                    dic.Add("total", ProcessTotal);
                    dic.Add("number", ProcessNumber);
                    dic.Add("enddate", ProcessLastDate==null?ProcessLastDate:DateTime.Parse(ProcessLastDate).ToCnDataString());
                }
                else
                {
                    dic.Add("percentage", 100);
                    dic.Add("total", ProcessTotal);
                    dic.Add("number", ProcessNumber);
                    dic.Add("enddate", ProcessLastDate == null ? ProcessLastDate : DateTime.Parse(ProcessLastDate).ToCnDataString());
                }
                return dic;
            }
           
            
        }
    }
}
