using Evious.Ims.Contract.chukou1;
using Evious.Ims.Contract.Model;
using Evious.Ims.Contract.Model.PayPal;
using Evious.Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Evious.Ims.Contract.Model.CK1BFE
{
    class CK1DirectExpressResponse : Evious.Ims.Contract.Model.CK1APIV3.API_V3_Response
    {

        [JsonProperty(PropertyName = "body")]
        new public List<CK1DirectExpress> body { get; set; }
    }

    class CK1OutboundExpressResponse : Evious.Ims.Contract.Model.CK1APIV3.API_V3_Response
    {

        [JsonProperty(PropertyName = "body")]
        new public List<CK1OutboundExpress> body { get; set; }
    }

    class PriceResponse : Evious.Ims.Contract.Model.CK1APIV3.API_V3_Response
    {

        [JsonProperty(PropertyName = "body")]
        new public Express body { get; set; }
    }
    public class CK1BFE : ICarrier
    {
       
        public CK1BFE()
        { 
           
        }


        #region transactiondetail
        /// <summary>
        /// 国内直发
        /// </summary>
        /// <param name="form">订单表单</param>
        /// <param name="transactiondetail">交易对象</param>
        /// <returns>订单对象</returns>
        /// 
        public ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, TransactionDetail transactiondetail)
        {
            #region 生成包裹

            var product = new List<ExpressProduct>();

            foreach (var item in transactiondetail.TransactionItems)
            {
                product.Add(new ExpressProduct
                {
                    CustomsTitleCN = "怀表",
                    CustomsTitleEN = "POCKET WATCH",              
                    Quantity = item.ItemQty,
                    SKU = item.Association.Product.Sku,
                    Weight =item.Association.Product.Weight,
                    DeclareValue = dispatchrequest.goodsDeclareWorth / transactiondetail.TransactionItems.Count
                });
            }
            

            var packageList = new List<ExpressPackage>();
            packageList.Add(new ExpressPackage
            {
                Custom = dispatchrequest.Custom,
                ShipToAddress = new ShipToAddress
                {
                    Contact = dispatchrequest.ShiptoName,
                    Street1 = dispatchrequest.ShipToStreet,
                    Street2 = dispatchrequest.ShipToStreet2,
                    City = dispatchrequest.ShiptoCity,
                    Province = dispatchrequest.ShipToState,
                    Country = dispatchrequest.ShipToCountryName,
                    PostCode = dispatchrequest.ShipToZip,
                },
                Packing = new Evious.Ims.Contract.chukou1.Packing
                {
                    Height = dispatchrequest.Height,
                    Length = dispatchrequest.Length,
                    Width = dispatchrequest.Width,
                },
                Remark = "备注信息",
                Status = OrderExpressState.Initial,
                TrackCode = "",
                Weight = dispatchrequest.goodsWeight,
                ProductList = product.ToArray(),
            });

            var orderDetail = new ExpressOrder
            {
                IsTracking = false,
                Location = "GZ",
                Remark = "测试订单",
                PackageList = packageList.ToArray(),
            };

            #endregion
           
            ShipOrder order = new ShipOrder();
            var clint = new CK1();           
            var request = new ExpressAddOrderNewRequest
            {
                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                Submit = false,
                OrderDetail = orderDetail,
                ExpressTypeNew = dispatchrequest.Express,              
            };

            ExpressAddOrderResponse response = clint.ExpressAddOrderNew(request);
            try
            {
                order = new ShipOrder
                {
                    Message = response.Message,
                    OrderSign = response.OrderSign
                };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return order;
        }
        
        public ShipOrder AddOutboundOrder(DispatchRequest dispatchrequest, TransactionDetail transactiondetail)
        {
            ShipOrder order = new ShipOrder();
            CK1 clint = new CK1();
            #region 生成包裹
            
            var productList = new List<OutStoreProduct>();

            foreach (var item in transactiondetail.TransactionItems)
            {
                productList.Add(new OutStoreProduct()
                {
                    SKU = item.Association.Product.Sku,
                    Quantity = item.ItemQty,
                });
            }

            var packageList = new List<OutStorePackage>();
            packageList.Add(new OutStorePackageNew()
            {

                ProductList = productList.ToArray(),
                ShipToAddress = new ShipToAddress()
                {

                    Contact = dispatchrequest.ShiptoName,
                    Country = dispatchrequest.ShipToCountryName,
                    PostCode = dispatchrequest.ShipToZip,
                    Province = dispatchrequest.ShipToState,
                    Street1 = dispatchrequest.ShipToStreet,
                    Street2 = dispatchrequest.ShipToStreet2,
                    City = dispatchrequest.ShiptoCity,
                },
                Shipping = OutStoreShipping.None,
                ShippingV2_1 = ConvertOutStoreShippingV2_1(dispatchrequest.Express.ToUpper())
            });


            #endregion 生成包裹

            var request = new OutStoreAddOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                Submit = false,
                OrderDetail = new OutStoreOrder()
                {
                    State = OrderOutState.Initial,
                    PackageList = packageList.ToArray(),
                },
            };

            switch (dispatchrequest.Warehouse.ToUpper())
            {
                case "US":
                    request.OrderDetail.Warehouse = EnumWarehouse.US;
                    break;
                case "AU":
                    request.OrderDetail.Warehouse = EnumWarehouse.AU;
                    break;
                case "UK":
                    request.OrderDetail.Warehouse = EnumWarehouse.UK;
                    break;
                case "NJ":
                    request.OrderDetail.Warehouse = EnumWarehouse.NJ;
                    break;
                case "DE":
                    request.OrderDetail.Warehouse = EnumWarehouse.DE;
                    break;
            }

            request.Submit = false;
            var response = clint.OutStoreAddOrder(request);
            try
            {
                order = new ShipOrder
                {
                    Message = response.Message,
                    OrderSign = response.OrderSign
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
            CK1 clint = new CK1();
            var request = new ExpressCompleteOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                ActionType=EnumActionType.Cancel,
                OrderSign=transactiondetail.OrderSign,
            };
            ExpressCompleteOrderResponse response = clint.ExpressCompleteOrder(request);
            return response.Message;
        }

        public string SubmitExpressOrder(TransactionDetail transactiondetail)
        {
            CK1 clint = new CK1();
            var request = new ExpressCompleteOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                ActionType = EnumActionType.Submit,
                OrderSign = transactiondetail.OrderSign,
            };
            ExpressCompleteOrderResponse response = clint.ExpressCompleteOrder(request);
            return response.Message;
        }

        #endregion

        #region paymenttransaction
        public ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, PaymentTransactionType paymenttransaction)
        {
            #region 生成包裹

            var product = new List<ExpressProduct>();

            foreach (var item in paymenttransaction.PaymentItemInfo.PaymentItem)
            {
                product.Add(new ExpressProduct
                {
                    CustomsTitleCN = "怀表",
                    CustomsTitleEN = "POCKET WATCH",
                    Quantity = Convert.ToInt32(item.Quantity),
                    SKU = item.Association.Product.Sku,
                    Weight = item.Association.Product.Weight,
                    DeclareValue = dispatchrequest.goodsDeclareWorth / paymenttransaction.PaymentItemInfo.PaymentItem.Count
                });
            }


            var packageList = new List<ExpressPackage>();
            packageList.Add(new ExpressPackage
            {
                Custom = dispatchrequest.Custom,
                ShipToAddress = new ShipToAddress
                {
                    Contact = dispatchrequest.ShiptoName,
                    Street1 = dispatchrequest.ShipToStreet,
                    Street2 = dispatchrequest.ShipToStreet2,
                    City = dispatchrequest.ShiptoCity,
                    Province = dispatchrequest.ShipToState,
                    Country = dispatchrequest.ShipToCountryName,
                    PostCode = dispatchrequest.ShipToZip,
                },
                Packing = new Evious.Ims.Contract.chukou1.Packing
                {
                    Height = dispatchrequest.Height,
                    Length = dispatchrequest.Length,
                    Width = dispatchrequest.Width,
                },
                Remark = "备注信息",
                Status = OrderExpressState.Initial,
                TrackCode = "",
                Weight = dispatchrequest.goodsWeight,
                ProductList = product.ToArray(),
            });

            var orderDetail = new ExpressOrder
            {
                IsTracking = false,
                Location = "GZ",
                Remark = "测试订单",
                PackageList = packageList.ToArray(),
            };

            #endregion

            ShipOrder order = new ShipOrder();
            var clint = new CK1();
            var request = new ExpressAddOrderNewRequest
            {
                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                Submit = false,
                OrderDetail = orderDetail,
                ExpressTypeNew = dispatchrequest.Express,
            };

            ExpressAddOrderResponse response = clint.ExpressAddOrderNew(request);
            try
            {
                order = new ShipOrder
                {
                    Message = response.Message,
                    OrderSign = response.OrderSign
                };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return order;
        }

        public ShipOrder AddOutboundOrder(DispatchRequest dispatchrequest, PaymentTransactionType paymenttransaction)
        {
            ShipOrder order = new ShipOrder();
            CK1 clint = new CK1();
            #region 生成包裹

            var productList = new List<OutStoreProduct>();

            foreach (var item in paymenttransaction.PaymentItemInfo.PaymentItem)
            {
                productList.Add(new OutStoreProduct()
                {
                    SKU = item.Association.Product.Sku,
                    Quantity =Convert.ToInt32(item.Quantity),
                });
            }

            var packageList = new List<OutStorePackage>();
            packageList.Add(new OutStorePackageNew()
            {

                ProductList = productList.ToArray(),
                ShipToAddress = new ShipToAddress()
                {

                    Contact = dispatchrequest.ShiptoName,
                    Country = dispatchrequest.ShipToCountryName,
                    PostCode = dispatchrequest.ShipToZip,
                    Province = dispatchrequest.ShipToState,
                    Street1 = dispatchrequest.ShipToStreet,
                    Street2 = dispatchrequest.ShipToStreet2,
                    City = dispatchrequest.ShiptoCity,
                },
                Shipping = OutStoreShipping.None,
                ShippingV2_1 = ConvertOutStoreShippingV2_1(dispatchrequest.Express.ToUpper())
            });


            #endregion 生成包裹

            var request = new OutStoreAddOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                Submit = false,
                OrderDetail = new OutStoreOrder()
                {
                    State = OrderOutState.Initial,
                    PackageList = packageList.ToArray(),
                },
            };

            switch (dispatchrequest.Warehouse.ToUpper())
            {
                case "US":
                    request.OrderDetail.Warehouse = EnumWarehouse.US;
                    break;
                case "AU":
                    request.OrderDetail.Warehouse = EnumWarehouse.AU;
                    break;
                case "UK":
                    request.OrderDetail.Warehouse = EnumWarehouse.UK;
                    break;
                case "NJ":
                    request.OrderDetail.Warehouse = EnumWarehouse.NJ;
                    break;
                case "DE":
                    request.OrderDetail.Warehouse = EnumWarehouse.DE;
                    break;
            }

            request.Submit = false;
            var response = clint.OutStoreAddOrder(request);
            try
            {
                order = new ShipOrder
                {
                    Message = response.Message,
                    OrderSign = response.OrderSign
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
            CK1 clint = new CK1();
            var request = new ExpressCompleteOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                ActionType = EnumActionType.Cancel,
                OrderSign = paymenttransaction.OrderSign,
            };
            ExpressCompleteOrderResponse response = clint.ExpressCompleteOrder(request);
            return response.Message;
        }

        public string SubmitExpressOrder(PaymentTransactionType paymenttransaction)
        {
            CK1 clint = new CK1();
            var request = new ExpressCompleteOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                ActionType = EnumActionType.Submit,
                OrderSign = paymenttransaction.OrderSign,
            };
            ExpressCompleteOrderResponse response = clint.ExpressCompleteOrder(request);
            return response.Message;
        }
        #endregion

        #region PayPalTransaction

        public ShipOrder AddExpressOrder(DispatchRequest dispatchrequest, PayPalTransaction paypaltransaction)
        {
            #region 生成包裹

            var product = new List<ExpressProduct>();

            foreach (var item in paypaltransaction.PayPalTransactionPaymentItems)
            {
                product.Add(new ExpressProduct
                {
                    CustomsTitleCN = "怀表",
                    CustomsTitleEN = "POCKET WATCH",
                    Quantity = Convert.ToInt32(item.PaymentItemQuantity),
                    SKU = item.Association.Product.Sku,
                    Weight = item.Association.Product.Weight,
                    DeclareValue = dispatchrequest.goodsDeclareWorth / paypaltransaction.PayPalTransactionPaymentItems.Count
                });
            }


            var packageList = new List<ExpressPackage>();
            packageList.Add(new ExpressPackage
            {
                Custom = dispatchrequest.Custom,
                ShipToAddress = new ShipToAddress
                {
                    Contact = dispatchrequest.ShiptoName,
                    Street1 = dispatchrequest.ShipToStreet,
                    Street2 = dispatchrequest.ShipToStreet2,
                    City = dispatchrequest.ShiptoCity,
                    Province = dispatchrequest.ShipToState,
                    Country = dispatchrequest.ShipToCountryName,
                    PostCode = dispatchrequest.ShipToZip,
                },
                Packing = new Evious.Ims.Contract.chukou1.Packing
                {
                    Height = dispatchrequest.Height,
                    Length = dispatchrequest.Length,
                    Width = dispatchrequest.Width,
                },
                Remark = "备注信息",
                Status = OrderExpressState.Initial,
                TrackCode = "",
                Weight = dispatchrequest.goodsWeight,
                ProductList = product.ToArray(),
            });

            var orderDetail = new ExpressOrder
            {
                IsTracking = false,
                Location = "GZ",
                Remark = "测试订单",
                PackageList = packageList.ToArray(),
            };

            #endregion

            ShipOrder order = new ShipOrder();
            var clint = new CK1();
            var request = new ExpressAddOrderNewRequest
            {
                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                Submit = false,
                OrderDetail = orderDetail,
                ExpressTypeNew = dispatchrequest.Express,
            };

            ExpressAddOrderResponse response = clint.ExpressAddOrderNew(request);
            try
            {
                order = new ShipOrder
                {
                    Message = response.Message,
                    OrderSign = response.OrderSign
                };
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return order;
        }
        public ShipOrder AddOutboundOrder(DispatchRequest dispatchrequest, PayPalTransaction paypaltransaction)
        {
            #region 生成包裹

            var product = new List<ExpressProduct>();

            foreach (var item in paypaltransaction.PayPalTransactionPaymentItems)
            {
                product.Add(new ExpressProduct
                {
                    CustomsTitleCN = "怀表",
                    CustomsTitleEN = "POCKET WATCH",
                    Quantity = Convert.ToInt32(item.PaymentItemQuantity),
                    SKU = item.Association.Product.Sku,
                    Weight = item.Association.Product.Weight,
                    DeclareValue = dispatchrequest.goodsDeclareWorth / paypaltransaction.PayPalTransactionPaymentItems.Count
                });
            }


            var packageList = new List<ExpressPackage>();
            packageList.Add(new ExpressPackage
            {
                Custom = dispatchrequest.Custom,
                ShipToAddress = new ShipToAddress
                {
                    Contact = dispatchrequest.ShiptoName,
                    Street1 = dispatchrequest.ShipToStreet,
                    Street2 = dispatchrequest.ShipToStreet2,
                    City = dispatchrequest.ShiptoCity,
                    Province = dispatchrequest.ShipToState,
                    Country = dispatchrequest.ShipToCountryName,
                    PostCode = dispatchrequest.ShipToZip,
                },
                Packing = new Evious.Ims.Contract.chukou1.Packing
                {
                    Height = dispatchrequest.Height,
                    Length = dispatchrequest.Length,
                    Width = dispatchrequest.Width,
                },
                Remark = "备注信息",
                Status = OrderExpressState.Initial,
                TrackCode = "",
                Weight = dispatchrequest.goodsWeight,
                ProductList = product.ToArray(),
            });

            var orderDetail = new ExpressOrder
            {
                IsTracking = false,
                Location = "GZ",
                Remark = "测试订单",
                PackageList = packageList.ToArray(),
            };

            #endregion

            ShipOrder order = new ShipOrder();
            var clint = new CK1();
            var request = new ExpressAddOrderNewRequest
            {
                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                Submit = false,
                OrderDetail = orderDetail,
                ExpressTypeNew = dispatchrequest.Express,
            };

            ExpressAddOrderResponse response = clint.ExpressAddOrderNew(request);
            try
            {
                order = new ShipOrder
                {
                    Message = response.Message,
                    OrderSign = response.OrderSign
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
            CK1 clint = new CK1();
            var request = new ExpressCompleteOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                ActionType = EnumActionType.Cancel,
                OrderSign = paypaltransaction.OrderSign,
            };
            ExpressCompleteOrderResponse response = clint.ExpressCompleteOrder(request);
            return response.Message;
        }

        public string SubmitExpressOrder(PayPalTransaction paypaltransaction)
        {
            CK1 clint = new CK1();
            var request = new ExpressCompleteOrderRequest()
            {

                UserKey = CK1Config.getUserKey(),
                Token = CK1Config.getToken(),
                ActionType = EnumActionType.Submit,
                OrderSign = paypaltransaction.OrderSign,
            };
            ExpressCompleteOrderResponse response = clint.ExpressCompleteOrder(request);
            return response.Message;
        }
        #endregion


        private static OutStoreShippingV2_1 ConvertOutStoreShippingV2_1(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("can't be null or empty.", "name");
            }
            OutStoreShippingV2_1 eenum;
            var result = System.Enum.TryParse<OutStoreShippingV2_1>(name, true, out eenum);
            if (!result)
                throw new ArgumentException(" Invalid", "name");

            return eenum;
        }

        /// <summary>
        /// 快递列表
        /// </summary>
        /// <returns>快递</returns>
        public IEnumerable<Express> GetExpressList()
        { 
            List<Express> list = new List<Express>();

            CK1V3Request request = new CK1V3Request
            {
                Category = "direct-express",
                Handler = "misc",
                Action = "list-all-service",
                Parameters = new Dictionary<string, string>()
            };
            string json = ResponseJson(request);
            CK1DirectExpressResponse response = JsonConvert.DeserializeObject<CK1DirectExpressResponse>(json);

            try
            {

                foreach (var service in response.body)
                {
                    list.Add(new Express
                    {
                        Code = service.Code,
                        Name = service.Name
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return list;
        }
        public IEnumerable<Express> GetOutBoundList(string warehouse)
        {
            List<Express> list = new List<Express>();

            CK1V3Request request = new CK1V3Request
            {
                Category = "Outbound",
                Handler = "misc",
                Action = "list-all-service",
                Parameters = new Dictionary<string, string>()
            };
            request.Parameters.Add("warehouse", warehouse);
            string json = ResponseJson(request);
            CK1OutboundExpressResponse response = JsonConvert.DeserializeObject<CK1OutboundExpressResponse>(json);

            try
            {

                foreach (var service in response.body)
                {
                    list.Add(new Express
                    {
                        Code = service.Code,
                        Name = service.Name
                    });
                }
            }
            catch (Exception ex)
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
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("Packing", requst.Packing.Length.ToString()+"*"+requst.Packing.Width.ToString()+"*"+requst.Packing.Height.ToString());
            param.Add("weight_in_gram", requst.Weight.ToString());
            param.Add("Country", requst.ShipToCountryName);
            param.Add("service", requst.ExpressService);

            CK1V3Request request = new CK1V3Request
            {
                Category = "direct-express",
                Handler = "package",
                Action = "pricing",
                Parameters = param
            };
            //Packing packing = new Packing();
            
            string json = ResponseJson(request);
            PriceResponse response = JsonConvert.DeserializeObject<PriceResponse>(json);
            SummaryInfo summary = new SummaryInfo();

            summary = response.body.Summary[0];

            return summary;
        }

        private string ResponseJson(CK1V3Request request)
        {
            Dictionary<string, string> Dispatcher =
                             new Dictionary<string, string>()
                        {
                            {"category", request.Category},
                            {"handler", request.Handler},
                        };
            Dispatcher.Add("action", request.Action);

            var requestUrl = Evious.Ims.Contract.Model.CK1APIV3.CK1API_SDK_Base.CreateRequestUrl(Dispatcher);            
            string json = HttpHelper.HttpGet(requestUrl, request.Parameters);
            return json;
        }




        
    }
}