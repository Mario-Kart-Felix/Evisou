using Evisou.Ims.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Evisou.Ims.Contract.sfc;
using Evisou.Ims.Contract.Model.PayPal;

namespace Evisou.Ims.Contract.Model.SFC
{
    public class SFCAPI : ICarrier
    {
        private static readonly string userKey = "AAAAB3NzaC1yc2EAAAABJQAAAIEAxiiVHSi9ewXvOleAfREoJP/5ID6uxN88Ee/eYAJN7aHrcg3dztNessxAr7bAZjW3r0fKL3sw6nL+YjITfh+I3xwrwLpkuEAX6EDAx4y8aofxyUz27qCRBdjpdpiTLOEDrR2n1pdtbU1eYlk7vAUoGOFg+BGCGbhmVazsMtVhkxM";

        private static readonly string token = "AAAAB3NzaC1yc2EAAAABJQAAAIBv9cJPnh4K8UwN4zGvM2d6kv4S3nNcgyzgBYgDwQR/Gg88dTWq8TXmNVn5RjekItdUVlp38f+5HUY9vzPs1oRmjjLbYvKGqEztUeyNkjU6x6BZbC9UaG1nVwfbU15vD+yjodIsBRntu+98j3qTWkB/596l80A2PdDVEbVDBE9edQ";

        private static readonly string userId = "Z1L50";

        private HeaderRequest HR = new HeaderRequest();

        /// <summary>
        /// 构造函数
        /// </summary>
        public SFCAPI()
        {
            HR = new HeaderRequest() { appKey = userKey, userId = userId, token = token };
                               
        }


        #region transactiondetail
        /// <summary>
        /// 国内直发
        /// </summary>
        /// <param name="form">订单表单</param>
        /// <param name="transactiondetail">交易对象</param>
        /// <returns>订单对象</returns>
        public ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, TransactionDetail transactiondetail)
        {
            //string service = form["Service"].ToUpper();

            ShipRateSOAP s = new ShipRateSOAP();
            addOrderRequestInfoArray addOrderRequestInfo = new addOrderRequestInfoArray();

            addOrderRequestInfo.shippingMethod = dispatchrequest.Express;
            addOrderRequestInfo.recipientCountry = dispatchrequest.ShipToCountryName;
            addOrderRequestInfo.recipientName = dispatchrequest.ShiptoName;
            addOrderRequestInfo.recipientAddress = dispatchrequest.ShipToStreet;
            addOrderRequestInfo.recipientZipCode = dispatchrequest.ShipToZip;
            addOrderRequestInfo.recipientCity = dispatchrequest.ShiptoCity;
            addOrderRequestInfo.recipientState = string.IsNullOrEmpty(dispatchrequest.ShipToState) ? dispatchrequest.ShiptoCity : dispatchrequest.ShipToState;

            addOrderRequestInfo.orderStatus = "confirmed";//"preprocess";
            addOrderRequestInfo.recipientPhone = "123456963";
            addOrderRequestInfo.recipientEmail = transactiondetail.Email;
            addOrderRequestInfo.goodsDescription = dispatchrequest.goodsDescription;
            addOrderRequestInfo.goodsQuantity ="1";//
           // addOrderRequestInfo.goodsQuantitySpecified = true;
            addOrderRequestInfo.goodsDeclareWorth = dispatchrequest.goodsDeclareWorth.ToString();//
           // addOrderRequestInfo.goodsDeclareWorthSpecified = true;
            addOrderRequestInfo.isReturn = "1";
          
            addOrderRequestInfo.evaluate = (dispatchrequest.goodsDeclareWorth + 1M).ToString();
            //addOrderRequestInfo.evaluateSpecified = true;

            List<goodsDetailsArray> list = new List<goodsDetailsArray>();
            list.Add(new goodsDetailsArray
            {
                detailWorth = dispatchrequest.goodsDeclareWorth.ToString(),
                detailQuantity = "1",
                //detailQuantitySpecified = true,
                detailDescription = dispatchrequest.goodsDescription,
                //detailWorthSpecified = true
            });
            addOrderRequestInfo.goodsDetails = list.ToArray<goodsDetailsArray>();

            string customerOrderNo, operatingTime,orderActionStatus, exceptionCode, notifyUrl,note , alert,area, trackingNumber;

            ShipOrder order = new ShipOrder();

            /*  var request = s.addOrder(
                  HR,
                  addOrderRequestInfo,
                  out customerOrderNo,
                  out operatingTime,
                  out orderActionStatus,
                  out exceptionCode,
                  out notifyUrl,
                  out note,
                  out alert
                  );
   order = new ShipOrder
              {
                  Message = note,
                  OrderSign = request.ToString()
              };

              order.Message = note;
              order.OrderSign =request??request.ToString();

              var aa = order;*/
            try
            {
                var request = s.addOrder(
                HR,
                addOrderRequestInfo,
                out customerOrderNo,
                out operatingTime,
                out orderActionStatus,
                out exceptionCode,
                out notifyUrl,
                out note,
                out alert,
                out area,
                out trackingNumber
                );

                order = new ShipOrder
                {
                    Message = note,
                    OrderSign = request ?? request.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return order;
        }

        public string DeleteExpressOrder(TransactionDetail transactiondetail)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            UpdateOrderStatusInfoArray updateOrderRequestInfo = new UpdateOrderStatusInfoArray();
            updateOrderRequestInfo.orderStatus = "deleted";

            string message = "";
            // string time;
            DateTime time;
            var request = s.updateOrderStatus(
                HR,
                updateOrderRequestInfo,
                transactiondetail.OrderSign,
                out message,
                out time
                );
            return message;
        }
        public string SubmitExpressOrder(TransactionDetail transactiondetail)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            UpdateOrderStatusInfoArray updateOrderRequestInfo = new UpdateOrderStatusInfoArray();
            updateOrderRequestInfo.orderStatus = "sumbmitted";

            string message = "";
            //string time;
            DateTime time;
            var request = s.updateOrderStatus(
                HR,
                updateOrderRequestInfo,
                transactiondetail.OrderSign,
                out message,
                out time
                );
            return message;
        }

        #endregion

        #region paymenttransaction
        public ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, PaymentTransactionType paymenttransaction)
        {
            //string service = form["Service"].ToUpper();

            ShipRateSOAP s = new ShipRateSOAP();
            addOrderRequestInfoArray addOrderRequestInfo = new addOrderRequestInfoArray();
            
            addOrderRequestInfo.shippingMethod = dispatchrequest.Express;
            addOrderRequestInfo.recipientCountry = dispatchrequest.ShipToCountryName;
            addOrderRequestInfo.recipientName = dispatchrequest.ShiptoName;
            addOrderRequestInfo.recipientAddress = dispatchrequest.ShipToStreet;
            addOrderRequestInfo.recipientZipCode = dispatchrequest.ShipToZip;
            addOrderRequestInfo.recipientCity = dispatchrequest.ShiptoCity;
            addOrderRequestInfo.recipientState = string.IsNullOrEmpty(dispatchrequest.ShipToState) ? dispatchrequest.ShiptoCity : dispatchrequest.ShipToState;

            addOrderRequestInfo.orderStatus = "confirmed";//"preprocess";
            addOrderRequestInfo.recipientPhone = "123456963";
            addOrderRequestInfo.recipientEmail = paymenttransaction.PayerInfo.Payer;
            addOrderRequestInfo.goodsDescription = dispatchrequest.goodsDescription;
            addOrderRequestInfo.goodsQuantity = "1";//
            //addOrderRequestInfo.goodsQuantitySpecified = true;
            addOrderRequestInfo.goodsDeclareWorth = dispatchrequest.goodsDeclareWorth.ToString();//
            //addOrderRequestInfo.goodsDeclareWorthSpecified = true;
            addOrderRequestInfo.isReturn = "1";
            addOrderRequestInfo.evaluate = (dispatchrequest.goodsDeclareWorth+ 1M).ToString();
            //addOrderRequestInfo.evaluateSpecified = true;

            List<goodsDetailsArray> list = new List<goodsDetailsArray>();
            list.Add(new goodsDetailsArray
            {
                detailWorth = dispatchrequest.goodsDeclareWorth.ToString(),
                detailQuantity = "1",
               // detailQuantitySpecified = true,
                detailDescription = dispatchrequest.goodsDescription,
                //detailWorthSpecified = true
            });
            addOrderRequestInfo.goodsDetails = list.ToArray<goodsDetailsArray>();

            string customerOrderNo, operatingTime, orderActionStatus, exceptionCode, notifyUrl, note, alert, area, trackingNumber;
            ShipOrder order = new ShipOrder();

            /*  var request = s.addOrder(
                  HR,
                  addOrderRequestInfo,
                  out customerOrderNo,
                  out operatingTime,
                  out orderActionStatus,
                  out exceptionCode,
                  out notifyUrl,
                  out note,
                  out alert
                  );
   order = new ShipOrder
              {
                  Message = note,
                  OrderSign = request.ToString()
              };

              order.Message = note;
              order.OrderSign =request??request.ToString();

              var aa = order;*/
            try
            {
                var request = s.addOrder(
                HR,
                addOrderRequestInfo,
                out customerOrderNo,
                out operatingTime,
                out orderActionStatus,
                out exceptionCode,
                out notifyUrl,
                out note,
                out alert,
                out area,
                out trackingNumber
                );

                order = new ShipOrder
                {
                    Message = note,
                    OrderSign = request ?? request.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return order;
        }

        public string DeleteExpressOrder(PaymentTransactionType paymenttransaction)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            UpdateOrderStatusInfoArray updateOrderRequestInfo = new UpdateOrderStatusInfoArray();
            updateOrderRequestInfo.orderStatus = "deleted";

            string message = "";
            //string time;
            DateTime time;
            
            var request = s.updateOrderStatus(
                HR,
                updateOrderRequestInfo,
                paymenttransaction.OrderSign,
                out message,
                out time
                );
            return message;
        }

        public string SubmitExpressOrder(PaymentTransactionType paymenttransaction)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            UpdateOrderStatusInfoArray updateOrderRequestInfo = new UpdateOrderStatusInfoArray();
            updateOrderRequestInfo.orderStatus = "sumbmitted";

            string message = "";
            // string time;
            DateTime time;
            var request = s.updateOrderStatus(
                HR,
                updateOrderRequestInfo,
                paymenttransaction.OrderSign,
                out message,
                out time
                );
            return message;
        }
        #endregion


        #region paypaltransaction
        public ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, PayPalTransaction paypaltransaction)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            addOrderRequestInfoArray addOrderRequestInfo = new addOrderRequestInfoArray();

            addOrderRequestInfo.shippingMethod = dispatchrequest.Express;
            addOrderRequestInfo.recipientCountry = dispatchrequest.ShipToCountryName;
            addOrderRequestInfo.recipientName = dispatchrequest.ShiptoName;
            addOrderRequestInfo.recipientAddress = dispatchrequest.ShipToStreet;
            addOrderRequestInfo.recipientZipCode = dispatchrequest.ShipToZip;
            addOrderRequestInfo.recipientCity = dispatchrequest.ShiptoCity;
            addOrderRequestInfo.recipientState = string.IsNullOrEmpty(dispatchrequest.ShipToState) ? dispatchrequest.ShiptoCity : dispatchrequest.ShipToState;

            addOrderRequestInfo.orderStatus = "confirmed";//"preprocess";
            addOrderRequestInfo.recipientPhone = "123456963";
            addOrderRequestInfo.recipientEmail = paypaltransaction.Receiver;
            addOrderRequestInfo.goodsDescription = dispatchrequest.goodsDescription;
            addOrderRequestInfo.goodsQuantity = "1";//
            //addOrderRequestInfo.goodsQuantitySpecified = true;
            addOrderRequestInfo.goodsDeclareWorth = dispatchrequest.goodsDeclareWorth.ToString();//
           // addOrderRequestInfo.goodsDeclareWorthSpecified = true;
            addOrderRequestInfo.isReturn = "1";
            addOrderRequestInfo.evaluate = (dispatchrequest.goodsDeclareWorth+1M).ToString();
            //addOrderRequestInfo.evaluateSpecified = true;

            List<goodsDetailsArray> list = new List<goodsDetailsArray>();
            list.Add(new goodsDetailsArray
            {
                detailWorth = dispatchrequest.goodsDeclareWorth.ToString(),
                detailQuantity = "1",
               // detailQuantitySpecified = true,
                detailDescription = dispatchrequest.goodsDescription,
               // detailWorthSpecified = true
            });
            addOrderRequestInfo.goodsDetails = list.ToArray<goodsDetailsArray>();

            string customerOrderNo, operatingTime, orderActionStatus, exceptionCode, notifyUrl, note, alert, area, trackingNumber;
            ShipOrder order = new ShipOrder();

            /*  var request = s.addOrder(
                  HR,
                  addOrderRequestInfo,
                  out customerOrderNo,
                  out operatingTime,
                  out orderActionStatus,
                  out exceptionCode,
                  out notifyUrl,
                  out note,
                  out alert
                  );
   order = new ShipOrder
              {
                  Message = note,
                  OrderSign = request.ToString()
              };

              order.Message = note;
              order.OrderSign =request??request.ToString();

              var aa = order;*/
            try
            {
                var request = s.addOrder(
                HR,
                addOrderRequestInfo,
                out customerOrderNo,
                out operatingTime,
                out orderActionStatus,
                out exceptionCode,
                out notifyUrl,
                out note,
                out alert,
                out area,
                out trackingNumber
                );

                order = new ShipOrder
                {
                    Message = note,
                    OrderSign = request ?? request.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return order;
        }

        public string DeleteExpressOrder(PayPalTransaction paypaltransaction)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            UpdateOrderStatusInfoArray updateOrderRequestInfo = new UpdateOrderStatusInfoArray();
            updateOrderRequestInfo.orderStatus = "deleted";

            string message = "";
            DateTime time;

            var request = s.updateOrderStatus(
                HR,
                updateOrderRequestInfo,
                paypaltransaction.OrderSign,
                out message,
                out time
                );
            return message;
        }

        public string SubmitExpressOrder(PayPalTransaction paypaltransaction)
        {
            ShipRateSOAP s = new ShipRateSOAP();
            UpdateOrderStatusInfoArray updateOrderRequestInfo = new UpdateOrderStatusInfoArray();
            updateOrderRequestInfo.orderStatus = "sumbmitted";

            string message = "";
            //string time;
            DateTime time;
            var request = s.updateOrderStatus(
                HR,
                updateOrderRequestInfo,
                paypaltransaction.OrderSign,
                out message,
                out time
                );
            return message;
        }
        #endregion
        /// <summary>
        /// 快递列表
        /// </summary>
        /// <returns>快递</returns>
        public IEnumerable<Express> GetExpressList()
        {
            ShipRateSOAP s = new ShipRateSOAP(); 
            List<Express> list = new List<Express>();
            try
            {
                shiptype[] c = s.getShipTypes(HR, 0, false);   
                foreach (var service in c)
                {
                    list.Add(new Express
                    {
                        Code = service.method_code,
                        Name = service.cn_name
                    });
                }               
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
             return list;

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentcode"></param>
        /// <param name="express_service"></param>
        /// <param name="sku"></param>
        /// <param name="warehouse"></param>
        /// <param name="to_region"></param>
        /// <param name="to_country"></param>
        /// <param name="to_zip_code"></param>
        /// <param name="to_city"></param>
        /// <param name="weight"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SummaryInfo GetDirectExpressShippingFee(PriceRequest requst)
        {
            ShipRateSOAP s = new ShipRateSOAP();
           // requst.Weight = requst.Weight / 1000;
            //string[] si = size.Split('*');

            getRateByModeRequestInfo getratebymode = new getRateByModeRequestInfo
            {
                country = requst.ShipToCountryName,
                length = float.Parse(requst.Packing.Length.ToString()),
                lengthSpecified = true,
                width = float.Parse(requst.Packing.Width.ToString()),
                widthSpecified = true,
                height = float.Parse(requst.Packing.Height.ToString()),
                heightSpecified = true,
                weight = requst.Weight / 1000,
                weightSpecified = true,
                mode = requst.ExpressService
            };
            float totalfee; bool totalfeeSpecified; float costfee; bool costfeeSpecified; float base_fee; bool base_feeSpecified;
            float dealfee; bool dealfeeSpecified; float regfee; bool regfeeSpecified; float addons;
            bool addonsSpecified; string deliverytime; string isweight = ""; string iftracking = ""; string classtype = "";
            string classtypecode = ""; string shiptypecode = ""; string shiptypename = ""; string shiptypecnname = "";
            SummaryInfo summary = new SummaryInfo();
            try
            {
                s.getRateByMode(HR, getratebymode, out totalfee,
                    out totalfeeSpecified, out costfee, out costfeeSpecified, out base_fee, out base_feeSpecified, out dealfee, out dealfeeSpecified, out regfee, out regfeeSpecified, out addons,
                    out addonsSpecified, out deliverytime, out isweight, out iftracking, out classtype, out classtypecode, out shiptypecode, out shiptypename, out shiptypecnname
                    );
                summary.Money = totalfee.ToString();
                return summary;
               
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            
        }

        
    }
}