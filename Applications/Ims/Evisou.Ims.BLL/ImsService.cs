using EntityFramework.Extensions;
using Evisou.Core.Log;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using Evisou.Framework.Web;
using Evisou.Ims.Contract;
using Evisou.Ims.Contract.Enum;
using Evisou.Ims.Contract.Model;
using Evisou.Ims.Contract.Model.CK1BFE;
using Evisou.Ims.Contract.Model.SFC;
using Evisou.Ims.DAL;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Evisou.Ims.BLL
{
    public static class PredicateBuilder 
    {

        /// <summary>
        /// 机关函数应用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混应时写在AND后的OR有效 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 机关函数应用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混应时写在OR后面的AND有效 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
    public class ImsService : IImsService, IPayPalHelper
    {
        #region Product CURD
        public Product GetProduct(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
               // return dbContext.Find<Product>(id);
                return dbContext.Products.Include(m=>m.Images).FirstOrDefault(a => a.ID == id);
            }
        }
        public IEnumerable<Product> GetProductList(ProductRequest request = null)
        {
            request = request ?? new ProductRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<Product> products = dbContext.Products.Include(m=>m.Images);
                if (!string.IsNullOrEmpty(request.Name))
                    products = products.Where(u => u.Name.ToLower()==request.Name.ToLower());

                if (!string.IsNullOrEmpty(request.Sku))
                    products = products.Where(u => u.Sku.Contains(request.Sku));

                if (request.IDs != null)
                {
                    products = products.Where(u => request.IDs.Contains(u.ID));
                }


                return products.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SaveProduct(Product product)
        {
            using (var dbContext = new ImsDbContext())
            {

                List<Image> images = new List<Image>();

                

                if (product.PictureURL != null)
                {
                    #region 有上传的图片的情况
                   
                    if (product.Images != null)
                    {
                        #region 当数据库有Image的情况
                        for (int i = 0; i < product.PictureURL.Count(); i++)
                        {
                            int id = product.PictureID[i];

                            if (id != 0)
                            {
                                var existImage = dbContext.Images.FirstOrDefault(t => t.ID == id);
                                existImage.Label = product.Label[i].ToString();
                                existImage.PictureURL = product.PictureURL[i].ToString();
                                existImage.SortOrder = int.Parse(product.SortOrder[i].ToString());
                                images.Add(existImage);
                            }
                            else
                            {
                                images.Add(new Image
                                {

                                    CreateTime = DateTime.Now,
                                    Label = product.Label[i].ToString(),
                                    PictureURL = product.PictureURL[i].ToString(),
                                    SortOrder = int.Parse(product.SortOrder[i].ToString()),
                                });
                            }


                        }
                        #endregion                        

                    }
                    else
                    {
                        #region 当数据库没有Image的情况
                        for (int i = 0; i < product.PictureURL.Count(); i++)
                        {
                            images.Add(new Image
                            {

                                CreateTime = DateTime.Now,
                                Label = product.Label[i].ToString(),
                                PictureURL = product.PictureURL[i].ToString(),
                                SortOrder = int.Parse(product.SortOrder[i].ToString()),
                            });
                        }
                        #endregion

                    }
                    #endregion

                }
                //else
                //{
                //    #region 没有上传图的情况
                //    if (product.Images != null)
                //    {
                //        foreach (var image in product.Images)
                //        {
                //            var existImage = dbContext.Images.FirstOrDefault(t => t.ID == image.ID);
                //            images.Add(existImage ?? image);
                //        }
                //    }
                //    #endregion
                    
                //}


                if (product.ID > 0)
                {
                    product.SelectedImages = new int[] { };                    
                    //product.Images = new List<Image>();
                    dbContext.Update<Product>(product);

                    dbContext.Entry(product).Collection(m => m.Images).Load();
                    product.Images = images;
                    dbContext.SaveChanges();
                }
                else
                {
                    if (product.PictureURL != null)
                    {
                        for (int i = 0; i < product.PictureURL.Count(); i++)
                        {
                            images.Add(new Image
                            {
                                CreateTime = DateTime.Now,
                                Label = product.Label[i].ToString(),
                                PictureURL = product.PictureURL[i].ToString(),
                                SortOrder = int.Parse(product.SortOrder[i].ToString()),
                            });
                        }
                    }
                    
                    product.Images = images;                  
                    dbContext.Insert<Product>(product);
                }


                //List<Image> images = new List<Image>(); 
                //if (product.ID > 0)
                //{    
                //    if (product.PictureURL != null)
                //    {
                //        for (int i = 0; i < product.PictureURL.Count(); i++)
                //        {
                //            images.Add(new Image
                //            {
                                
                //                CreateTime = DateTime.Now,
                //                Label = product.Label[i].ToString(),
                //                PictureURL = product.PictureURL[i].ToString(),
                //                SortOrder = int.Parse(product.SortOrder[i].ToString()),
                //               // ProductID=product.ID,
                //            });
                //        }
                //    }
                //    product.Images = new List<Image>();                   
                //    dbContext.Update<Product>(product);

                //    dbContext.Entry(product).Collection(m => m.Images).Load();
                //    product.Images = images;    
                //    dbContext.SaveChanges();
                //}
                //else
                //{
                //    if (product.PictureURL != null)
                //    {
                //        for (int i = 0; i < product.PictureURL.Count(); i++)
                //        {
                //            images.Add(new Image
                //            {
                //                CreateTime = DateTime.Now,
                //                Label = product.Label[i].ToString(),
                //                PictureURL = product.PictureURL[i].ToString(),
                //                SortOrder = int.Parse(product.SortOrder[i].ToString()),
                //               // ProductID = product.ID,
                //            });
                //        }
                //    }
                //    product.Images = images;
                //    dbContext.Insert<Product>(product);
                //}
            }
        }
        public void DeleteProduct(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Products.Include(m => m.Images).Where(u => ids.Contains(u.ID))
                    .ToList().ForEach(a => { a.Images.Clear(); dbContext.Products.Remove(a); });
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region Image CURD

        public Image GetImage(string imagepath)
        {
            using (var dbContext = new ImsDbContext())
            {
               var image= dbContext.Images.Where(u => u.PictureURL.ToLower() == imagepath.ToLower()).FirstOrDefault<Image>();
               return image;
            }
        }

        public void DeleteImage(string imagepath)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Images.Where(u =>u.PictureURL.ToLower()==imagepath.ToLower()).Delete();
            }
        }
        #endregion

        #region Supplier CURD
        public Supplier GetSupplier(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                //  return dbContext.Find<Supplier>(id);
                return dbContext.Suppliers.Include("Products").FirstOrDefault(a => a.ID == id);
            }
        }
        public IEnumerable<Supplier> GetSupplierList(SupplierRequest request = null)
        {
            request = request ?? new SupplierRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<Supplier> suppliers = dbContext.Suppliers.AsNoTracking();
                if (!string.IsNullOrEmpty(request.Name))
                    suppliers = suppliers.Where(u => u.Name.Contains(request.Name)).AsNoTracking();
                return suppliers.OrderByDescending(u => u.ID).AsNoTracking().ToList();//.ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SaveSupplier(Supplier supplier)
        {
            using (var dbContext = new ImsDbContext())
            {

                var products = new List<Product>();

                if (supplier.Products != null)
                {
                    foreach (var product in supplier.Products)
                    {
                        var existProduct = dbContext.Products.FirstOrDefault(t => t.ID == product.ID);
                        products.Add(existProduct ?? product);
                    }
                }

                if (supplier.ID > 0)
                {
                    supplier.SelectedProduct = new int[] { };
                    dbContext.Update<Supplier>(supplier);


                    dbContext.Entry(supplier).Collection(m => m.Products).Load();
                    supplier.Products = products;
                    dbContext.SaveChanges();
                }
                else
                {
                    supplier.Products = products;
                    dbContext.Insert<Supplier>(supplier);
                }

            }
        }
        public void DeleteSupplier(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Suppliers.Where(u => ids.Contains(u.ID)).Delete();
            }
        }
        #endregion

        #region Purchase CURD
        public Purchase GetPurchase(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                return dbContext.Find<Purchase>(id);
            }
        }
        public IEnumerable<Purchase> GetPurchaseList(PurchaseRequest request = null)
        {
            request = request ?? new PurchaseRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<Purchase> Purchases = dbContext.Purchases.AsNoTracking();
                if (!string.IsNullOrEmpty(request.PurchaseTransactionID))
                    Purchases = Purchases.Where(u => u.PurchaseTransactionID.Contains(request.PurchaseTransactionID)).AsNoTracking();


                return Purchases.OrderByDescending(u => u.ID).AsNoTracking().ToList();//.ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SavePurchase(Purchase purchase)
        {
            using (var dbContext = new ImsDbContext())
            {
                List<PurchaseProduct> purchaseproucts = new List<PurchaseProduct>();
                if (purchase.ID > 0)
                {
                    /*supplier.SelectedProduct = new int[] { };
                    dbContext.Update<Supplier>(supplier);
                    dbContext.Entry(supplier).Collection(m => m.Products).Load();*/
                    if (purchase.selectedProduct != null)
                    {
                        for (int i = 0; i < purchase.selectedProduct.Count(); i++)
                        {
                            purchaseproucts.Add(new PurchaseProduct
                            {
                                ProductID = purchase.selectedProduct[i],
                                Quantity = purchase.selectedQuantity[i],
                                UnitPrice = purchase.selectedPrice[i],
                                PurchaseID = purchase.ID
                            });
                        }
                    }
                    purchase.PurchaseProducts = new List<PurchaseProduct>();
                    dbContext.Update<Purchase>(purchase);


                    dbContext.Entry(purchase).Collection(m => m.PurchaseProducts).Load();
                    purchase.PurchaseProducts = purchaseproucts;
                    dbContext.SaveChanges();
                }
                else
                {
                    if (purchase.selectedProduct != null)
                    {
                        for (int i = 0; i < purchase.selectedProduct.Count(); i++)
                        {
                            purchaseproucts.Add(new PurchaseProduct
                            {
                                ProductID = purchase.selectedProduct[i],
                                Quantity = purchase.selectedQuantity[i],
                                UnitPrice = purchase.selectedPrice[i],

                            });
                        }
                    }

                    purchase.PurchaseProducts = purchaseproucts;
                    dbContext.Insert<Purchase>(purchase);
                }
            }
        }
        public void DeletePurchase(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Purchases.Where(u => ids.Contains(u.ID)).Delete();
            }
        }

        public IEnumerable<Product> getProductBySupplier(int id, int? purchaseid)
        {
            using (var dbContext = new ImsDbContext())
            {
                Purchase purchase = new Purchase();

                HashSet<int> HS = new HashSet<int>();
                HashSet<PurchaseProduct> purchaseProductHS = new HashSet<PurchaseProduct>();
                List<Product> viewModel = new List<Product>();
                if (!string.IsNullOrEmpty(purchaseid.ToString()))
                {
                    purchase = dbContext.Purchases.Include(i => i.PurchaseProducts).Where(i => i.ID == purchaseid).Single();
                    HS = new HashSet<int>(purchase.PurchaseProducts.Select(c => c.ProductID));
                    purchaseProductHS = new HashSet<PurchaseProduct>(purchase.PurchaseProducts);

                }
                //var supplierAllProducts = dbContext.Suppliers.Include(i => i.Products).Select(x => x.Products).Single();
                var supplierAllProducts = dbContext.Suppliers.Where(c=>c.ID==id).Include(i => i.Products).Select(x => x.Products).Single();
                foreach (var product in supplierAllProducts)
                {
                    PurchaseProduct purchaseproduct = purchaseProductHS.Where(c => c.ProductID == product.ID && c.PurchaseID == purchase.ID).FirstOrDefault();
                    product.Quantity = purchaseproduct == null ? 0 : purchaseproduct.Quantity;
                    product.UnitPrice = purchaseproduct == null ? 0 : purchaseproduct.UnitPrice;
                    product.Assigned = purchaseproduct == null ? false : true;
                    viewModel.Add(product);

                }
                return viewModel;
            }
        }
        #endregion

        #region Agent CURD
        public Agent GetAgent(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                return dbContext.Find<Agent>(id);
            }
        }
        public IEnumerable<Agent> GetAgentList(AgentRequest request = null)
        {
            request = request ?? new AgentRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<Agent> agents = dbContext.Agents;
                if (!string.IsNullOrEmpty(request.AgentName))
                    agents = agents.Where(u => u.AgentName.Contains(request.AgentName));
                return agents.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SaveAgent(Agent agent)
        {
            using (var dbContext = new ImsDbContext())
            {
                if (agent.ID > 0)
                {
                    dbContext.Update<Agent>(agent);
                }
                else
                {
                    dbContext.Insert<Agent>(agent);
                }
            }
        }
        public void DeleteAgent(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Agents.Where(u => ids.Contains(u.ID)).Delete();
            }
        }
        #endregion

        #region GetPaymentTransaction
        public Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType GetPaymentTransaction(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                var transactiondetails = dbContext.PayPalPaymentTransactions.Include(i => i.PaymentInfo)
                                                                            .Include(a => a.PayerInfo)                                                                            
                                                                            .Include(c => c.PayerInfo.Address)
                                                                            .Include(c => c.PaymentItemInfo)
                                                                            .Include(c => c.PaymentItemInfo.Auction)
                                                                            .Include(c => c.PaymentItemInfo.Subscription)
                                                                            .Include(c => c.PaymentItemInfo.PaymentItem)
                                                                           // .Include(c => c.PaymentItemInfo.PaymentItem)
                                                                            .Include(c=>c.ReceiverInfo)
                                                                            .Where(c => c.ID == id)
                                                                            .FirstOrDefault<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType>();
                List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType> PaymentItems = new List<Contract.Model.PayPal.PaymentItemType>();


                transactiondetails.PaymentItemInfo.PaymentItem.ToList().ForEach(a => { 
                    var association = dbContext.Associations.Include(i => i.Product).Where(i => i.ID == a.AssociationID).FirstOrDefault();
                    a.Association = association;
                });

                return transactiondetails;
            }



        }

        public IEnumerable<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType> GetPaymentTransactionList(PaymentTransactionRequest request = null)
        {
            request = request ?? new PaymentTransactionRequest();

            using (var dbContext = new ImsDbContext())
            {
                IQueryable<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType> paymenttransactionlist =
                    dbContext.PayPalPaymentTransactions.Include(c => c.PaymentInfo)
                                                       //.Include(c=>c.PaymentInfo.FeeAmount)
                                                       //.Include(c=>c.PaymentInfo.GrossAmount)
                                                       //.Include(c=>c.PaymentInfo.SettleAmount)
                                                       //.Include(c=>c.PaymentInfo.TaxAmount)
                                                       //.Include(c => c.PaymentInfo.InstrumentDetails)
                                                       //.Include(c => c.PaymentInfo.TransactionType)
                                                        .Include(c => c.PayerInfo)
                                                        .Include(c=>c.PayerInfo.Address)
                                                        //.Include(c=>c.PayerInfo.PayerCountry)
                                                        .Include(c => c.PaymentItemInfo)
                                                        .Include(c=>c.PaymentItemInfo.PaymentItem)
                                                        .Include(c=>c.PaymentItemInfo.Auction)
                                                        .Include(c=>c.PaymentItemInfo.Subscription)
                                                        .Include(c=>c.ReceiverInfo);
                
                


                if (!string.IsNullOrEmpty(request.TransactionID))
                    paymenttransactionlist = paymenttransactionlist.Where(u => u.PaymentInfo.TransactionID.Contains(request.TransactionID));
                return paymenttransactionlist.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

        public void SavePaymentTransaction(Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction)
        {
            using (var dbContext = new ImsDbContext())
            {
                if (paymenttransaction.ID > 0)
                {
                    dbContext.Update<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType>(paymenttransaction);
                }
                else
                {
                    dbContext.Insert<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType>(paymenttransaction);
                }
            }
        }

        public void DeletePaymentTransaction(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.PayPalPaymentTransactions.Where(u => ids.Contains(u.ID)).Delete();
            }
        }

        public List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType> DistinctPaymentItemInfo()
        {
            var mItems = new HashSet<string>(this.GetAssociationList().Select(c => c.ItemTitle));

            List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType> TransactionItems = new List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType>();

            foreach (Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType detail in this.GetPaymentTransactionList())
            {
                if (detail.PaymentItemInfo != null)
                {
                    foreach (Evisou.Ims.Contract.Model.PayPal.PaymentItemType item in detail.PaymentItemInfo.PaymentItem)
                    {
                        if (!mItems.Contains(item.Name))
                        {
                            TransactionItems.Add(new Evisou.Ims.Contract.Model.PayPal.PaymentItemType
                            {
                                Number=item.Number,
                                Name = item.Name
                            });
                        }
                    }
                }
            }
            return TransactionItems;
        }


        private List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType> GetPaymentItemsInfo()
        {
            List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType> paymentitems = new List<Contract.Model.PayPal.PaymentItemType>();

            foreach (Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType detail in this.GetPaymentTransactionList())
            {
                if (detail.PaymentItemInfo != null)
                {
                    foreach (Evisou.Ims.Contract.Model.PayPal.PaymentItemType item in detail.PaymentItemInfo.PaymentItem)
                    {
                        paymentitems.Add(item);
                    }
                }
            }

            return paymentitems;
        }
        #endregion

        #region PayPalTransaction
        public PayPalTransaction GetPayPalTransaction(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                var transactiondetails = dbContext.PayPalTransactions.Include(i => i.PayPalTransactionPaymentItems)                                                                            
                                                                            .Where(c => c.ID == id).AsNoTracking()
                                                                            .FirstOrDefault<PayPalTransaction>();
                dbContext.Configuration.AutoDetectChangesEnabled = true;

                List<PayPalTransactionPaymentItem> PaymentItems = new List<PayPalTransactionPaymentItem>();


                transactiondetails.PayPalTransactionPaymentItems.ToList().ForEach(a =>
                {
                    var association = dbContext.Associations.Include(i => i.Product).Where(i => i.ID == a.AssociationID).FirstOrDefault();
                    a.Association = association;
                });

                return transactiondetails;
            }
        }
        public IEnumerable<PayPalTransaction> GetPayPalTransactionList(PayPalTransactionRequest request = null)
        {
            request = request ?? new PayPalTransactionRequest();

            using (var dbContext = new ImsDbContext())
            {
                IQueryable<PayPalTransaction> paymenttransactionlist =
                    dbContext.PayPalTransactions.Include(c => c.PayPalTransactionPaymentItems).AsNoTracking();


                if (!string.IsNullOrEmpty(request.TransactionId))
                    paymenttransactionlist = paymenttransactionlist.Where(u => u.TransactionId.Contains(request.TransactionId)).AsNoTracking();
                return paymenttransactionlist.OrderByDescending(u => u.ID).AsNoTracking().ToList();//.ToPagedList(request.PageIndex, request.PageSize);//.ToList<PayPalTransaction>();
            }
        }

        public Task<List<PayPalTransaction>> GetPayPalTransactionListIQueryable(PayPalTransactionRequest request = null)
        {
            request = request ?? new PayPalTransactionRequest();

            using (var dbContext = new ImsDbContext())
            {
                IQueryable<PayPalTransaction> paymenttransactionlist =
                    dbContext.PayPalTransactions.Include(c => c.PayPalTransactionPaymentItems).AsNoTracking();


                if (!string.IsNullOrEmpty(request.TransactionId))
                    paymenttransactionlist = paymenttransactionlist.Where(u => u.TransactionId.Contains(request.TransactionId)).AsNoTracking().OrderByDescending(u=>u.PaymentDate);
                return paymenttransactionlist.ToListAsync();//.ToPagedList(request.PageIndex, request.PageSize);//.ToList<PayPalTransaction>();
            }
        }
        public async Task<IEnumerable<PayPalTransaction>> GetPayPalTransactionListAsync(PayPalTransactionRequest request = null)
        {
            request = request ?? new PayPalTransactionRequest();

            using (var dbContext = new ImsDbContext())
            {
                var paymenttransactionlist =
                    dbContext.PayPalTransactions.Include(c => c.PayPalTransactionPaymentItems).AsNoTracking();

                if (!string.IsNullOrEmpty(request.TransactionId))
                    paymenttransactionlist = paymenttransactionlist.Where(u => u.TransactionId.Contains(request.TransactionId)).AsNoTracking();

                return await paymenttransactionlist.OrderByDescending(u => u.ID).AsNoTracking().ToListAsync<PayPalTransaction>(); 
                //List<PayPalTransaction> asynclist = paymenttransactionlist.OrderByDescending(u => u.ID).AsNoTracking().ToListAsync<PayPalTransaction>();
            }
        }
        public async Task<IEnumerable<PayPalTransaction>> GetPayPalTransactionListAsync2(PayPalTransactionRequest request = null)
        {
            request = request ?? new PayPalTransactionRequest();
            using (var dbContext = new ImsDbContext())
            {
                var filter = PredicateBuilder.True<PayPalTransaction>();
                if (!string.IsNullOrEmpty(request.TransactionId))
                {
                    filter = filter.And(c => c.TransactionId.Contains(request.TransactionId));
                }

                if (!string.IsNullOrEmpty(request.PaymentStatus))
                {
                    filter = filter.And(c => c.PayerStatus.Contains(request.PaymentStatus));
                }
                return await dbContext.PayPalTransactions.Include(c => c.PayPalTransactionPaymentItems).Where(filter).AsNoTracking().ToListAsync();//.OrderByDescending(u => u.ID);

            } 
        }
        public void SavePayPalTransaction(PayPalTransaction paypaltransaction)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                if (paypaltransaction.ID > 0)
                {
                    dbContext.Update<PayPalTransaction>(paypaltransaction);
                }
                else
                {
                    dbContext.Insert<PayPalTransaction>(paypaltransaction);
                }
            }
        }
        public void DeletePayPalTransaction(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                dbContext.PayPalTransactions.Where(u => ids.Contains(u.ID)).Delete();
            }
        }

        public void PayPalTransactionDataExport(List<int> ids, string type)
        {
            using(var dbContext = new ImsDbContext())
            {
                
                
                List<object> TransactionList = new List<object>(); ;
                ids.ForEach(c =>
                {
                    var Tra = dbContext.PayPalTransactions
                        .Select(s => new { s.ID,s.PaymentDate,s.TransactionId,s.BuyerID
                            , s.PayerCountryName,s.GrossAmount,s.Agent,s.Express,})
                        .Where(a => a.ID == c).FirstOrDefault();
                    TransactionList.Add(Tra);
                });

                IList<object> paymenttransactionlist = TransactionList;
                var dataset=  IListDataSet.ToDataSet(paymenttransactionlist);                
                string[] oldcols = { "PaymentDate", "TransactionId", "BuyerID", "PayerCountryName", "GrossAmount", "Agent", "Express" };
                string[] newcols = {  "订单日期", "交易ID","买家ID","国家","销售价","物流代理","快递服务"};

                switch (type)
                {
                    case "excel":
                        NPOIHelper.ExportByWeb(dataset.Tables[0], "订单汇总", "订单汇总" + DateTime.Now.ToString("yyyyMMddHHmmss"), oldcols, newcols); 
                        break;
                    case "csv":
                        CSVFileHelper.ExportByWeb(dataset.Tables[0], "订单汇总", "订单汇总" + DateTime.Now.ToString("yyyyMMddHHmmss"), oldcols, newcols);
                        break;
                    case "xml":
                        XMLHelper.ExportByWeb(dataset.Tables[0], "订单汇总", "订单汇总" + DateTime.Now.ToString("yyyyMMddHHmmss"), oldcols, newcols);
                        //dataset.WriteXml
                        break;


                }
            }           
        }
        public void Datatable()
        {
            PayPalTransactionRequest request = new PayPalTransactionRequest();
            using (var dbContext = new ImsDbContext())
            {
                IQueryable<PayPalTransaction> allTransaction =
                     dbContext.PayPalTransactions.Include(c => c.PayPalTransactionPaymentItems).AsNoTracking();
                IEnumerable<PayPalTransaction> filterTransaction = allTransaction;

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
                //return Json(new
                //{
                //    draw = request.draw,//param.sEcho,
                //    recordsTotal = allTransaction.Count(),//alltransactions.Count(),
                //    recordsFiltered = filterTransaction.Count(),
                //    data = result
                //},
                //                 JsonRequestBehavior.AllowGet);
            }
        }

       
        #endregion

        #region TransactionDetail
        public TransactionDetail GetTransactionDetail(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                var transactiondetails = dbContext.TransactionDetails
                                                                    .Include(i => i.TransactionItems).Where(i => i.ID == id).FirstOrDefault<TransactionDetail>();
                var items = transactiondetails.TransactionItems;

                List<TransactionItem> TransactionItems = new List<TransactionItem>();
                foreach (var item in items)
                {
                    var association = dbContext.Associations.Include(i => i.Product).Where(i => i.ID == item.AssociationID).FirstOrDefault();
                    item.Association = association;
                    TransactionItems.Add(item);
                    /*Associations.Add(new Association(){
                      ID=(int)item.AssociationID,
                      ItemTitle=item.ItemTitle,
                      //PaypalApiID=item. 
                    })*/
                }
                transactiondetails.TransactionItems = TransactionItems;
                return transactiondetails;//dbContext.Find<TransactionDetail>(id);
            }
        }
        public IEnumerable<TransactionDetail> GetTransactionDetailList(TransactionDetailRequest request = null)
        {
            request = request ?? new TransactionDetailRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<TransactionDetail> transactiondetails = dbContext.TransactionDetails.Include("TransactionItems");
                if (!string.IsNullOrEmpty(request.TransactionID))
                    transactiondetails = transactiondetails.Where(u => u.TransactionID.Contains(request.TransactionID));
                return transactiondetails.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SaveTransactionDetail(TransactionDetail transactiondetail)
        {
            using (var dbContext = new ImsDbContext())
            {
                if (transactiondetail.ID > 0)
                {
                    dbContext.Update<TransactionDetail>(transactiondetail);
                }
                else
                {
                    dbContext.Insert<TransactionDetail>(transactiondetail);
                }
            }
        }
        public void DeleteTransactionDetail(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.TransactionDetails.Where(u => ids.Contains(u.ID)).Delete();
            }
        }

        public List<TransactionItem> DistinctTransactionItem()
        {
            var mItems = new HashSet<string>(this.GetAssociationList().Select(c => c.ItemTitle));

            List<TransactionItem> TransactionItems = new List<TransactionItem>();

            foreach (TransactionDetail detail in this.GetTransactionDetailList())
            {
                if (detail.TransactionItems != null)
                {
                    foreach (TransactionItem item in detail.TransactionItems)
                    {
                        if (!mItems.Contains(item.ItemTitle))
                        {
                            TransactionItems.Add(new TransactionItem
                            {
                                ItemTitle = item.ItemTitle
                            });
                        }
                    }
                }
            }
            return TransactionItems;
        }

        public List<TransactionDetail> GetCountryNameAndCode()
        {
            var countryCodesMapping = new Dictionary<string, RegionInfo>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var culture in cultures)
            {
                var region = new RegionInfo(culture.LCID);
                countryCodesMapping[region.ThreeLetterISORegionName] = region;
            }
            List<TransactionDetail> country = new List<TransactionDetail>();
            foreach (var mapping in countryCodesMapping.OrderBy(mapping => mapping.Key).Skip(2))
            {
                country.Add(new TransactionDetail()
                {
                    ShipToCountryName = mapping.Value.DisplayName + ":" + mapping.Value.EnglishName,
                    ShipToCountryCode = mapping.Value.TwoLetterISORegionName
                });
            }
            return country;
        }

        private List<TransactionItem> GetTransactionItems()
        {
            List<TransactionItem> TransactionItems = new List<TransactionItem>();

            foreach (TransactionDetail detail in this.GetTransactionDetailList())
            {
                if (detail.TransactionItems != null)
                {
                    foreach (TransactionItem item in detail.TransactionItems)
                    {
                        TransactionItems.Add(new TransactionItem
                        {
                            ID = item.ID,
                            ItemTitle = item.ItemTitle,
                            CountryCode = detail.ShipToCountryCode,
                            CreateTime = item.CreateTime,
                            ItemAmt = item.ItemAmt,
                            ItemID = item.ItemID,
                            ItemQty = item.ItemQty,
                            TransactionDetailID = item.TransactionDetailID,
                            AssociationID = item.AssociationID
                        });
                    }
                }
            }
            return TransactionItems;
        }
        public void Sync()
        {
            PayPalHelper(new PaypalApi());
            IEnumerable<PaymentTransactionSearchResultType> PaypalTransactionSearch = this.ApiTransactionSearch(DateTime.Parse("2014-03-01T15:33:36"), DateTime.Parse("2014-03-10T15:33:36"));
            var total = PaypalTransactionSearch.Count<PaymentTransactionSearchResultType>();
            foreach (var item in PaypalTransactionSearch)
            {

                GetTransactionDetailsResponseType PaypalTransactionDetails = this.ApiTransactionDetail(item);
                if (PaypalTransactionDetails.Ack.Equals(AckCodeType.SUCCESS))
                {
                    var detail = this.TransactionDetail(PaypalTransactionDetails);
                    using (var dbContext = new ImsDbContext())
                    {
                        TransactionDetail transactionDetail = new TransactionDetail();
                        dbContext.Insert<TransactionDetail>(detail);

                    }

                    //Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType detail = this.PaymentTransaction(PaypalTransactionDetails);
                    //using (var dbContext = new ImsDbContext())
                    //{
                    //    Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType transactionDetail = new Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType();
                    //   // dbContext.Insert<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType>(detail);

                    //    dbContext.Insert<Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType>(detail);

                    //}
                }
            }
        }
        #endregion

        #region PaypalApi
        public PaypalApi GetPaypalApi(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                PaypalApi paypalapi = dbContext.Find<PaypalApi>(id);
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                return paypalapi;
            }
        }
        public IEnumerable<PaypalApi> GetPaypalApiList(PaypalApiRequest request = null)
        {
            request = request ?? new PaypalApiRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<PaypalApi> paypalapis = dbContext.PaypalApis;
                if (!string.IsNullOrEmpty(request.ApiUserName))
                    paypalapis = paypalapis.Where(u => u.ApiUserName.Contains(request.ApiUserName));

               if(request.IsActive)
                  paypalapis = paypalapis.Where(u => u.IsActive==request.IsActive);
                return paypalapis.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SavePaypalApi(PaypalApi paypalapi)
        {
            using (var dbContext = new ImsDbContext())
            {
                if (paypalapi.ID > 0)
                {
                    dbContext.Update<PaypalApi>(paypalapi);
                }
                else
                {
                    dbContext.Insert<PaypalApi>(paypalapi);
                }
            }
        }
        public void DeletePaypalApi(List<int> ids)
        {
            using (var dbContext = new ImsDbContext())
            {
                dbContext.PaypalApis.Where(u => ids.Contains(u.ID)).Delete();
            }
        }

        #endregion

        #region PaypalHelper
        private static Dictionary<string, string> PayaplConfig { set; get; }
        public void PayPalHelper(PaypalApi paypalapi)
        {
            Dictionary<string, string> configurationMap = Configuration.GetAcctAndConfig();
            configurationMap.Add("account1.apiUsername", paypalapi.ApiUserName);
            configurationMap.Add("account1.apiPassword", paypalapi.ApiPassword);
            configurationMap.Add("account1.apiSignature", paypalapi.Signature);

            PayaplConfig = configurationMap;

        }

        public void PayPalHelper()
        {
            Dictionary<string, string> configurationMap = Configuration.GetAcctAndConfig();
            configurationMap.Add("account1.apiUsername", "vson.mail_api1.gmail.com");
            configurationMap.Add("account1.apiPassword", "SXRM84PQDRDF2NN8");
            configurationMap.Add("account1.apiSignature", "AiPC9BjkCyDFQXbSkoZcgqH3hpacAVh7dGhDzP86zF0PmJmF3H74KeiX");

            PayaplConfig = configurationMap;

        }
        /// <summary>
        /// 从Paypal API中搜索交易条数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回PaymentTransactionSearchResultType 的IEnumerable</returns>
        public IEnumerable<PaymentTransactionSearchResultType> ApiTransactionSearch(DateTime startDate, DateTime endDate)
        {
            TransactionSearchRequestType request = new TransactionSearchRequestType();

            TransactionSearchReq wrapper = new TransactionSearchReq();
            wrapper.TransactionSearchRequest = request;

            request.StartDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss");
            request.EndDate = endDate.ToString("yyyy-MM-ddTHH:mm:ss");
            request.TransactionClass = PaymentTransactionClassCodeType.RECEIVED;
            // Dictionary<string, string> configurationMap = Configuration.GetAcctAndConfig();
            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(PayaplConfig);
            TransactionSearchResponseType response = service.TransactionSearch(wrapper);
            return response.PaymentTransactions;

        }


        /// <summary>
        /// 从Paypal API中交易详细
        /// </summary>
        /// <param name="transctionDetail">PaymentTransactionSearchResultType</param>
        /// <returns>返回Paypal 交易详细</returns>
        public GetTransactionDetailsResponseType ApiTransactionDetail(PaymentTransactionSearchResultType transctionDetail)
        {
            GetTransactionDetailsRequestType request = new GetTransactionDetailsRequestType();
            request.TransactionID = transctionDetail.TransactionID;

            GetTransactionDetailsReq wrapper = new GetTransactionDetailsReq();
            wrapper.GetTransactionDetailsRequest = request;
            // Dictionary<string, string> configurationMap = Configuration.GetAcctAndConfig();
            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(PayaplConfig);
            GetTransactionDetailsResponseType transactionDetails = service.GetTransactionDetails(wrapper);

            return transactionDetails;
        }

        public Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType PaymentTransaction(GetTransactionDetailsResponseType getTransactionDetail)
        {
            DateTime now = DateTime.Now;
            Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType payment = new Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType();
            /* payment.BuyerEmailOptIn = getTransactionDetail.PaymentTransactionDetails.BuyerEmailOptIn;
             payment.GiftMessage = getTransactionDetail.PaymentTransactionDetails.GiftMessage;
             payment.GiftReceipt = getTransactionDetail.PaymentTransactionDetails.GiftReceipt;*/
            /*payment.GiftWrapAmount = getTransactionDetail.PaymentTransactionDetails.GiftWrapAmount!=null ?new Contract.Model.PayPal.BasicAmountType
            {
               // CreateTime = now,
                currencyID = getTransactionDetail.PaymentTransactionDetails.GiftWrapAmount.currencyID,
                value = getTransactionDetail.PaymentTransactionDetails.GiftWrapAmount.value
            }:null;*/
            //  payment.GiftWrapName = getTransactionDetail.PaymentTransactionDetails.GiftWrapName;
            // payment.OfferCouponInfo = getTransactionDetail.PaymentTransactionDetails.OfferCouponInfo;

            # region PayerInfo
            payment.PayerInfo = new Contract.Model.PayPal.PayerInfoType
            {
               Address = new Evisou.Ims.Contract.Model.PayPal.AddressType
                {
                    AddressNormalizationStatus = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressNormalizationStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressNormalizationStatus : null,
                    AddressID = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressID,
                    AddressOwner = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressOwner.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressOwner : null,
                    AddressStatus = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressStatus : null,
                    CityName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.CityName,
                    Country = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Country.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Country : null,
                    CountryName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.CountryName,
                    ExternalAddressID = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.ExternalAddressID,
                    InternationalName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.InternationalName,
                    InternationalStateAndCity = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.InternationalStateAndCity,
                    InternationalStreet = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.InternationalStreet,
                    Name = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Name,
                    Phone = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Phone,
                    PostalCode = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.PostalCode,
                    StateOrProvince = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.StateOrProvince,
                    Street1 = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Street1,
                    Street2 = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Street2
                },
                ContactPhone = getTransactionDetail.PaymentTransactionDetails.PayerInfo.ContactPhone,
                // CreateTime = now,
                //EnhancedPayerInfo = getTransactionDetail.PaymentTransactionDetails.PayerInfo.EnhancedPayerInfo,
                Payer = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Payer,
                PayerBusiness = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerBusiness,
                PayerCountry = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerCountry.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerCountry : null,
                /* PayerName = new Contract.Model.PayPal.PersonNameType
                 {
                     CreateTime = now,
                     LastName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.LastName,
                     FirstName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.FirstName,
                     MiddleName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.MiddleName,
                     Salutation = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.Salutation,
                     Suffix = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.Suffix
                 },*/
                // PayerStatus =getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerStatus.HasValue?getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerStatus:null,
                PayerID = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerID,
                /* TaxIdDetails = getTransactionDetail.PaymentTransactionDetails.PayerInfo.TaxIdDetails != null ? new Contract.Model.PayPal.TaxIdDetailsType
                 {
                     CreateTime = now,
                     TaxId = getTransactionDetail.PaymentTransactionDetails.PayerInfo.TaxIdDetails.TaxId,
                     TaxIdType = getTransactionDetail.PaymentTransactionDetails.PayerInfo.TaxIdDetails.TaxIdType
                 }:null*/
            };
            #endregion

            #region PaymentInfo
            payment.PaymentInfo = new Contract.Model.PayPal.PaymentInfoType
            {
                ParentTransactionID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ParentTransactionID,
                PaymentRequestID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentRequestID,
                PaymentStatus = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentStatus,
                PaymentType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType,
                PendingReason = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PendingReason,
                POSTransactionType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.POSTransactionType,
                ProtectionEligibility = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ProtectionEligibility,
                ProtectionEligibilityType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ProtectionEligibilityType,
                ReasonCode = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReasonCode,
                ReceiptID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReceiptID,
                ReceiptReferenceNumber = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReceiptReferenceNumber,
                RefundSourceCodeType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.RefundSourceCodeType,
                ShipAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipAmount,
                ShipDiscount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipDiscount,
                ShipHandleAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipHandleAmount,
                ShippingMethod = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShippingMethod,
                StoreID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.StoreID,
                Subject = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.Subject,
                TerminalID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TerminalID,
                TransactionID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TransactionID,
                TransactionType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TransactionType,
                BinEligibility = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.BinEligibility,                
                EbayTransactionID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.EbayTransactionID,                
                ExchangeRate = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ExchangeRate,
                ExpectedeCheckClearDate = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ExpectedeCheckClearDate,
                FeeAmount =getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount!=null? new Contract.Model.PayPal.BasicAmountType
                  {
                    
                      currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.currencyID,
                      value = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.value
                  }:null,
                 GrossAmount =getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount!=null? new Contract.Model.PayPal.BasicAmountType
                 {
                    
                     currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount.currencyID,
                     value = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount.value
                 }:null,               
                 SellerDetails=getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SellerDetails!=null?new Contract.Model.PayPal.SellerDetailsType
                 {
                     PayPalAccountID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SellerDetails.PayPalAccountID,
                     SecureMerchantAccountID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SellerDetails.SecureMerchantAccountID,
                     SellerId = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SellerDetails.SellerId,
                     SellerRegistrationDate = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SellerDetails.SellerRegistrationDate,
                     SellerUserName = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SellerDetails.SellerUserName
                 }:null,
                  InstrumentDetails=getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InstrumentDetails!=null?new Contract.Model.PayPal.InstrumentDetailsType
                  {
                      InstrumentCategory = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InstrumentDetails.InstrumentCategory,
                      InstrumentID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InstrumentDetails.InstrumentID
                  }:null,
                HoldDecision = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.HoldDecision,
                InsuranceAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InsuranceAmount,
                PaymentDate = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentDate,
                TaxAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount!=null?new Contract.Model.PayPal.BasicAmountType
                {
                    currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.currencyID,
                    value = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value
                }:new Contract.Model.PayPal.BasicAmountType
                {
                    currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.currencyID,
                    value = "0"//getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value
                },
                SettleAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SettleAmount != null ? new Contract.Model.PayPal.BasicAmountType
                {
                    currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SettleAmount.currencyID,
                    value = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SettleAmount.value
                } : new Contract.Model.PayPal.BasicAmountType
                {
                    currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.currencyID,
                    value = "0"//getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value
                },

            };

           



            /* if (getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SettleAmount != null)
            {
                payment.PaymentInfo.SettleAmount = new Contract.Model.PayPal.BasicAmountType
                {
                    currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SettleAmount.currencyID,
                    value = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.SettleAmount.value
                };
            }*/

            if (getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount != null)
            {
                payment.PaymentInfo.TaxAmount = new Contract.Model.PayPal.BasicAmountType
                {
                    currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.currencyID,
                    value = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value
                };
            }
            #endregion


            //List<Contract.Model.PayPal.SubscriptionTermsType> Terms = new List<Contract.Model.PayPal.SubscriptionTermsType>();
            //foreach (var item in getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.Terms)
            //{
            //    Terms.Add(new Contract.Model.PayPal.SubscriptionTermsType
            //    {
            //        /* Amount =item.Amount!=null? new Contract.Model.PayPal.BasicAmountType
            //         {
            //             //CreateTime = now,
            //             currencyID = item.Amount.currencyID,
            //             value = item.Amount.value,
            //         }:null,*/
            //        CreateTime = now,
            //        period = item.period
            //    });

            //}

            #region PaymentItems
            List<Contract.Model.PayPal.PaymentItemType> PaymentItems = new List<Contract.Model.PayPal.PaymentItemType>();
            foreach (var item in getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.PaymentItem)
            {
                 #region AdditionalFees
                //  List<Evisou.Ims.Contract.Model.PayPal.AdditionalFeeType> AdditionalFees = new List<Contract.Model.PayPal.AdditionalFeeType>();

                //  if (item.InvoiceItemDetails!=null)
                //  {
                //      foreach (var addtionalfee in item.InvoiceItemDetails.AdditionalFees)
                //      {
                //          AdditionalFees.Add(new Evisou.Ims.Contract.Model.PayPal.AdditionalFeeType()
                //          {
                //            /*  Amount = addtionalfee.Amount != null ? new Contract.Model.PayPal.BasicAmountType
                //              {
                //                  //CreateTime = now,
                //                  currencyID = addtionalfee.Amount.currencyID,
                //                  value = addtionalfee.Amount.value
                //              } : null,*/
                //              CreateTime = now,
                //              Type = addtionalfee.Type
                //          });
                //      };
                //  }

                #endregion

                 #region Discount
                // List<Evisou.Ims.Contract.Model.PayPal.DiscountType> Discount = new List<Contract.Model.PayPal.DiscountType>();

                // if (item.InvoiceItemDetails != null)
                // {
                //     foreach (var item_discount in item.InvoiceItemDetails.Discount)
                //     {
                //         Discount.Add(new Evisou.Ims.Contract.Model.PayPal.DiscountType
                //         {
                //            /* Amount = new Contract.Model.PayPal.BasicAmountType
                //             {
                //                // CreateTime = now,
                //                 currencyID = item_discount.Amount.currencyID,
                //                 value = item_discount.Amount.value,

                //             },*/
                //             CreateTime = now,
                //             Description = item_discount.Description,
                //             Name = item_discount.Name,
                //             RedeemedOfferID = item_discount.RedeemedOfferID,
                //             RedeemedOfferType = item_discount.RedeemedOfferType

                //         });
                //     }

                // }

                #endregion

                var PaymentItem = new Contract.Model.PayPal.PaymentItemType
                {

                    Amount = item.Amount != null ? new Contract.Model.PayPal.BasicAmountType
                    {
                        //  CreateTime = now,
                        currencyID = item.Amount.currencyID,
                        value = item.Amount.value,
                    } : new Contract.Model.PayPal.BasicAmountType
                    {
                        //  CreateTime = now,
                        currencyID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.currencyID,
                        value = "0",
                    },

                    CouponAmount = item.CouponAmount,
                    CouponAmountCurrency = item.CouponAmountCurrency,
                    CouponID = item.CouponID,
                    // CreateTime = now,
                    EbayItemTxnId = item.EbayItemTxnId,
                    LoyaltyCardDiscountAmount = item.LoyaltyCardDiscountAmount,
                    #region

                    //InvoiceItemDetails = item.InvoiceItemDetails != null ? new Contract.Model.PayPal.InvoiceItemType
                    //{
                    //    AdditionalFees = AdditionalFees,
                    //    CreateTime = now,
                    //    Description = item.InvoiceItemDetails.Description,
                    //    Discount = Discount,
                    //    EAN = item.InvoiceItemDetails.EAN,
                    //    ISBN = item.InvoiceItemDetails.ISBN,
                    //    ItemCount = item.InvoiceItemDetails.ItemCount,
                    //    /*  ItemPrice = item.InvoiceItemDetails.ItemPrice!=null?new Contract.Model.PayPal.BasicAmountType
                    //      {
                    //          //CreateTime = now,
                    //          currencyID = item.InvoiceItemDetails.ItemPrice.currencyID,
                    //          value = item.InvoiceItemDetails.ItemPrice.value
                    //      }:null,*/
                    //    ModelNumber = item.InvoiceItemDetails.ModelNumber,
                    //    ItemCountUnit = item.InvoiceItemDetails.ItemCountUnit,
                    //    MPN = item.InvoiceItemDetails.MPN,
                    //    Name = item.InvoiceItemDetails.Name,
                    //    PLU = item.InvoiceItemDetails.PLU,
                    //    /*   Price =item.InvoiceItemDetails.Price!=null?new Contract.Model.PayPal.BasicAmountType
                    //       {
                    //           //CreateTime = now,
                    //           currencyID = item.InvoiceItemDetails.Price.currencyID,
                    //           value = item.InvoiceItemDetails.Price.value
                    //       }:null,*/
                    //    Reimbursable = item.InvoiceItemDetails.Reimbursable,
                    //    ReturnPolicyIdentifier = item.InvoiceItemDetails.ReturnPolicyIdentifier,
                    //    SKU = item.InvoiceItemDetails.SKU,
                    //    StyleNumber = item.InvoiceItemDetails.StyleNumber,
                    //    Taxable = item.InvoiceItemDetails.Taxable,
                    //    TaxRate = item.InvoiceItemDetails.TaxRate,

                    //} : null,
                    #endregion
                    HandlingAmount = item.HandlingAmount,
                    LoyaltyCardDiscountCurrency = item.LoyaltyCardDiscountCurrency,
                    Name = item.Name,
                    Number = item.Number,
                    Quantity = item.Quantity,
                    SalesTax = item.SalesTax,
                    ShippingAmount = item.ShippingAmount,
                };
                Association Association = this.GetAssociationList(null).Where(i => i.ItemNumber == item.Number).FirstOrDefault();
                if (Association != null)
                {
                    PaymentItem.AssociationID = Association.ID;
                }
                PaymentItems.Add(PaymentItem);
            };
            #endregion

            payment.PaymentItemInfo = new Contract.Model.PayPal.PaymentItemInfoType
            {
                Auction = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Auction!=null? new Contract.Model.PayPal.AuctionInfoType
                {
                    BuyerID = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Auction.BuyerID,
                    ClosingDate = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Auction.ClosingDate,                    
                    multiItem = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Auction.multiItem

                }:null,               
                Custom = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Custom,
                InvoiceID = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.InvoiceID,
                Memo = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Memo,
                PaymentItem = PaymentItems,
                SalesTax = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.SalesTax,
                /* Subscription = new Contract.Model.PayPal.SubscriptionInfoType
                 {
                     CreateTime = now,
                     EffectiveDate = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.EffectiveDate,
                     Password = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.Password,
                     reattempt = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.reattempt,
                     Recurrences = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.Recurrences,
                     recurring = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.recurring,
                     RetryTime = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.RetryTime,
                     SubscriptionDate = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.SubscriptionDate,
                     SubscriptionID = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.SubscriptionID,
                     Terms = Terms,
                     Username = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Subscription.Username
                 },*/


            };
            payment.ReceiverInfo =getTransactionDetail.PaymentTransactionDetails.ReceiverInfo!=null? new Contract.Model.PayPal.ReceiverInfoType
              {
                  Business = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.Business,                 
                  Receiver = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.Receiver,
                  ReceiverID = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.ReceiverID
              }:null;
            /* payment.SecondaryAddress = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress!=null?new Contract.Model.PayPal.AddressType
             {
                 AddressID = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.AddressID,
                 AddressNormalizationStatus = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.AddressNormalizationStatus,
                 AddressOwner = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.AddressOwner,
                 AddressStatus = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.AddressStatus,
                 CityName = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.CityName,
                 Country = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.Country,
                 CountryName = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.CountryName,
                 CreateTime = now,
                 ExternalAddressID = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.ExternalAddressID,
                 InternationalName = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.InternationalName,
                 InternationalStateAndCity = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.InternationalStateAndCity,
                 InternationalStreet = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.InternationalStreet,
                 Name = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.Name,
                 Phone = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.Phone,
                 PostalCode = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.PostalCode,
                 StateOrProvince = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.StateOrProvince,
                 Street1 = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.Street1,
                 Street2 = getTransactionDetail.PaymentTransactionDetails.SecondaryAddress.Street2
             }:null;*/
            /*payment.SurveyChoiceSelected = getTransactionDetail.PaymentTransactionDetails.SurveyChoiceSelected;
            payment.SurveyQuestion = getTransactionDetail.PaymentTransactionDetails.SurveyQuestion;
            payment.TPLReferenceID = getTransactionDetail.PaymentTransactionDetails.TPLReferenceID;
            payment.UserSelectedOptions = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions!=null?new Contract.Model.PayPal.UserSelectedOptionType
            {
                CreateTime = now,
                InsuranceOptionSelected = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions.InsuranceOptionSelected,
                ShippingCalculationMode = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions.ShippingCalculationMode,
                ShippingOptionAmount = new Contract.Model.PayPal.BasicAmountType
                {
                    ///CreateTime = now,
                    currencyID = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions.ShippingOptionAmount.currencyID,
                    value = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions.ShippingOptionAmount.value,

                },
                ShippingOptionIsDefault = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions.ShippingOptionIsDefault,
                ShippingOptionName = getTransactionDetail.PaymentTransactionDetails.UserSelectedOptions.ShippingOptionName
            }:null;
        */

            return payment;
           

        }
        public TransactionDetail TransactionDetail(GetTransactionDetailsResponseType getTransactionDetail)
        {
            TransactionDetail detail = new TransactionDetail();
            List<TransactionItem> items = new List<TransactionItem>();
            detail.AddressOwner = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressOwner.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressOwner.Value.ToString() : null;
            detail.AddressStatus = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressStatus.Value.ToString() : null;
            detail.CountryCode = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerCountry.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerCountry.Value.ToString() : null;
            detail.PayerStatus = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerStatus.Value.ToString() : null;
            detail.PaymentStatus = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentStatus.Value.ToString() : null;
            detail.PaymentType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.Value.ToString() : null;
            detail.PendingReason = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PendingReason.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PendingReason.Value.ToString() : null;
            detail.ReasonCode = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReasonCode.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReasonCode.Value.ToString() : null;
            detail.ShipToCountryCode = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Country.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Country.Value.ToString() : null;
            detail.TransactionType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.Value.ToString() : null;
            detail.CurrencyCode = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount.currencyID.ToString();
            detail.Amt = decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount.value);
            detail.BuyerID = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Auction.BuyerID;
            detail.Email = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Payer;
            detail.FeeAmt = decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.value);
            detail.FirstName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.FirstName;
            detail.InsuranceAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InsuranceAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InsuranceAmount);
            detail.LastName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.LastName;
            detail.OrderTime = DateTime.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentDate);
            detail.ParentTransactionID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ParentTransactionID;
            detail.PayerID = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerID;
            detail.ProtectionEligibility = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ProtectionEligibility;
            detail.ReceiverBusiness = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.Business;
            detail.ReceiverEmail = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.Receiver;
            detail.ReceiverID = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.ReceiverID;
            detail.SaleTax = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value);
            detail.ShipHandleAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipHandleAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipHandleAmount);
            detail.ShiptoCity = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.CityName;
            detail.ShipToCountryName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.CountryName;
            detail.ShiptoName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Name;
            detail.ShipToState = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.StateOrProvince;
            detail.ShipToStreet = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Street1;
            detail.ShipToStreet2 = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Street2;
            detail.ShipToZip = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.PostalCode;
            detail.Subject = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.Subject;

            detail.TaxAmt = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value);
            detail.TransactionID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TransactionID;
            if (getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.PaymentItem.Count > 0)
            {
                foreach (PayPal.PayPalAPIInterfaceService.Model.PaymentItemType item in getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.PaymentItem)
                {
                    if (!string.IsNullOrEmpty(item.Quantity))
                    {
                        items.Add(
                        new TransactionItem
                        {
                            ItemAmt = decimal.Parse(item.Amount.value),
                            ItemID = item.Number,
                            ItemTitle = item.Name,
                            ItemQty = int.Parse(item.Quantity)
                        });
                    }

                }
                detail.TransactionItems = items;
            }


            return detail;

        }

        public PayPalTransaction PayPayTransaction(GetTransactionDetailsResponseType getTransactionDetail)
        {
            PayPalTransaction detail = new PayPalTransaction();
            detail.PayerAddressOwner = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressOwner.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressOwner.Value.ToString() : null;
            detail.PayerAddressStatus = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.AddressStatus.Value.ToString() : null;
            //detail.PayerCountry = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerCountry.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerCountry.Value.ToString() : null;
            detail.PayerStatus = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerStatus.Value.ToString() : null;
            detail.PaymentStatus = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentStatus.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentStatus.Value.ToString() : null;
            detail.PaymentType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.Value.ToString() : null;
            detail.PendingReason = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PendingReason.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PendingReason.Value.ToString() : null;
            //detail.PendingReason = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReasonCode.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ReasonCode.Value.ToString() : null;
            detail.PayerCountry = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Country.HasValue ? getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Country.Value.ToString() : null;
           // detail.TransactionType = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.HasValue ? getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentType.Value.ToString() : null;
            detail.CurrencyCode = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount.currencyID.ToString();
            detail.GrossAmount =getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount==null?0M:decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.GrossAmount.value);
            detail.BuyerID = getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.Auction.BuyerID;
            //detail.Receiver = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Payer;
            detail.FeeAmount =getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount==null?0M:decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.FeeAmount.value);
            detail.PayerFirstName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.FirstName;
           // detail.InsuranceAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InsuranceAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.InsuranceAmount);
            detail.PayerLastName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerName.LastName;
            detail.PaymentDate = DateTime.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.PaymentDate);
            detail.ParentTransactionID = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ParentTransactionID;
            detail.PayerID = getTransactionDetail.PaymentTransactionDetails.PayerInfo.PayerID;
           // detail.ProtectionEligibility = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ProtectionEligibility;
            detail.PayerBusiness = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.Business;
            detail.Receiver = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.Receiver;
            //detail.ReceiverID = getTransactionDetail.PaymentTransactionDetails.ReceiverInfo.ReceiverID;
           // detail.SaleTax = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value);
            //detail.ShipHandleAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipHandleAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.ShipHandleAmount);
            detail.PayerCityName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.CityName;
            detail.PayerCountryName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.CountryName;
            detail.PayerAddressName = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Name;
            detail.PayerStateOrProvince = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.StateOrProvince;
            detail.PayerAddressStreet1 = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Street1;
            detail.PayerAddressStreet2 = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.Street2;
            detail.PayerPostalCode = getTransactionDetail.PaymentTransactionDetails.PayerInfo.Address.PostalCode;
            detail.Subject = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.Subject;

            detail.TaxAmount = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount == null ? 0M : decimal.Parse(getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TaxAmount.value);
            detail.TransactionId = getTransactionDetail.PaymentTransactionDetails.PaymentInfo.TransactionID;

            List<PayPalTransactionPaymentItem> items = new List<PayPalTransactionPaymentItem>();

            getTransactionDetail.PaymentTransactionDetails.PaymentItemInfo.PaymentItem.ForEach(a =>
            {
                items.Add(new PayPalTransactionPaymentItem
                {
                    PaymentItemName = a.Name,
                    PaymentItemAmount =a.Amount==null?0M: decimal.Parse(a.Amount.value),
                    PaymentItemNumber = a.Number,
                    PaymentItemQuantity = a.Quantity,
                    PaymentItemEbayItemTxnId = a.EbayItemTxnId
                });
            });    
            
            
            detail.PayPalTransactionPaymentItems = items;
            return detail;
        }

        #endregion

        #region Association
        public Association GetAssociation(int id)
        {
            using (var dbContext = new ImsDbContext())
            {
                return dbContext.Find<Association>(id);
            }

        }
        public IEnumerable<Association> GetAssociationList(AssociationRequest request = null)
        {
            request = request ?? new AssociationRequest();

            using (var dbContext = new ImsDbContext())
            {

                IQueryable<Association> products = dbContext.Associations.Include("PaypalApi").Include("Product");
                if (!string.IsNullOrEmpty(request.ItemTitle))
                    products = products.Where(u => u.ItemTitle.Contains(request.ItemTitle));
                return products.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }
        public void SaveAssociation(Association association)
        {
            using (var dbContext = new ImsDbContext())
            {

                if (association.ID > 0)
                {
                    dbContext.Update<Association>(association);
                }
                else
                {
                    dbContext.Insert<Association>(association);
                }

                List<Evisou.Ims.Contract.Model.PayPal.PaymentItemType> PaymentItems = GetPaymentItemsInfo();
                PaymentItems.Where(i => i.Number == association.ItemNumber).ToList().ForEach(a =>
                {
                    a.AssociationID = association.ID;
                    dbContext.Update<Evisou.Ims.Contract.Model.PayPal.PaymentItemType>(a);
                });       


                //List<TransactionItem> TransactionItems = GetTransactionItems();
                //var items = TransactionItems.Where(i => i.ItemTitle == association.ItemTitle && i.CountryCode == association.StorePlace);
                //foreach (var item in items)
                //{
                //    item.AssociationID = association.ID;
                //    dbContext.Update<TransactionItem>(item);
                //}

               

                //foreach (var item in ItemList)
                //{
                //    item.AssociationID = association.ID;
                //    dbContext.Update<Evisou.Ims.Contract.Model.PayPal.PaymentItemType>(item);
                //}





            }
        }
        public void DeleteAssociation(List<int> ids)
        {

            using (TransactionScope transaction = new TransactionScope())
            {
                List<TransactionItem> TransactionItems = GetTransactionItems();
                using (var dbContext = new ImsDbContext())
                {
                    dbContext.Associations.Include(m => m.PaymentItems).Where(u => ids.Contains(u.ID))
                   .ToList().ForEach(a => { a.PaymentItems.Clear(); dbContext.Associations.Remove(a); });
                    dbContext.SaveChanges();
                    transaction.Complete();                             
                    //foreach (int id in ids)
                    //{
                    //    foreach (TransactionItem item in TransactionItems.Where(i => i.AssociationID == id))
                    //    {
                    //        item.AssociationID = null;
                    //        dbContext.Update<TransactionItem>(item);
                    //    }
                    //}
                    //dbContext.Associations.Where(u => ids.Contains(u.ID)).Delete();
                    //transaction.Complete();

                }
            }
        }

        #endregion

        #region Dispatch
        
        public IEnumerable<Express> GetDriectExpress(int agentid)
        {
            List<Express> express = new List<Express>();
            switch (agentid)
            {
                case 1:
                    CK1BFE ck1 = new CK1BFE();
                    express = ck1.GetExpressList().ToList<Express>();
                    break;

                case 2:
                    SFCAPI sfc = new SFCAPI();
                    express = sfc.GetExpressList().ToList<Express>();
                    break;

            }
            return express;
        }

        public IEnumerable<Warehouse> GetWarehouseList(int agentid)
        {
            List<Warehouse> wareshouselist = new List<Warehouse>();
            switch (agentid)
            {
                case 1:
                    CK1BFE ck1 = new CK1BFE();
                    wareshouselist = ck1.GetWarehouseList().ToList<Warehouse>();
                    break;


            }
            return wareshouselist;
        }
        public IEnumerable<Express> GetOutboundExpress(int agentid, string warehouse)
        {
            List<Express> express = new List<Express>();
            switch (agentid)
            {
                case 1:
                    CK1BFE ck1 = new CK1BFE();
                    express = ck1.GetOutBoundList(warehouse).ToList<Express>();
                    break;
            }

            return express;
        }

        #region transactiondetail
        public ShipOrder AddDispatchOrder(DispatchRequest request)
        {
            TransactionDetail detail = this.GetTransactionDetail(request.ID);
            string agent = Enum.GetName(typeof(ExpressAgentEnum), int.Parse(request.Agent));
            string type = Enum.GetName(typeof(ExpressTypeEnum), int.Parse(request.Type));
            ShipOrder shiporder = new ShipOrder();
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    #region 出口易
                    switch (type)
                    {
                        case "Outbound":
                            shiporder = ck1.AddOutboundOrder(request, detail);

                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                detail.Agent = ExpressAgentEnum.出口易;
                                detail.Express = request.Express;
                                detail.OrderSign = shiporder.OrderSign;
                                this.SaveTransactionDetail(detail);
                            }
                            break;

                        case "DirectExpress":

                            shiporder = ck1.AddExpressOrder(request, detail);
                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            { 
                                detail.Agent = ExpressAgentEnum.出口易;
                                detail.Express = request.Express;
                                detail.OrderSign = shiporder.OrderSign;
                                this.SaveTransactionDetail(detail);
                            }
                            
                            
                            break;
                    }
                    #endregion
                    break;

                case "三态速递"://sfc

                    SFCAPI sfc = new SFCAPI();
                    #region 三态速递
                    switch (type)
                    {
                        case "Outbound":
                            shiporder = new ShipOrder();
                            break;

                        case "DirectExpress":
                            shiporder = sfc.AddExpressOrder(request, detail);

                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                detail.Agent = ExpressAgentEnum.三态速递;
                                detail.Express = request.Express;
                                detail.OrderSign = shiporder.OrderSign;
                                this.SaveTransactionDetail(detail);
                            }
                            break;
                    }
                    #endregion 
                    break;
            }
            return shiporder;
        }

        public ShipOrder SaveDispatchOrder(DispatchRequest request) 
        {
            TransactionDetail detail = this.GetTransactionDetail(request.ID);
            string agent = Enum.GetName(typeof(ExpressAgentEnum), int.Parse(request.Agent));
            string type = Enum.GetName(typeof(ExpressTypeEnum), int.Parse(request.Type));
            ShipOrder shiporder = new ShipOrder();

            if (string.IsNullOrEmpty(detail.OrderSign))
            {
                switch (agent)
                {
                    case "出口易"://ck1
                        CK1BFE ck1 = new CK1BFE();
                        #region 出口易
                        switch (type)
                        {
                            case "Outbound":
                                shiporder = ck1.AddOutboundOrder(request, detail);

                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    detail.Agent = ExpressAgentEnum.出口易;
                                    detail.Express = request.Express;
                                    detail.OrderSign = shiporder.OrderSign;
                                    this.SaveTransactionDetail(detail);
                                }
                                break;

                            case "DirectExpress":

                                shiporder = ck1.AddExpressOrder(request, detail);
                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    detail.Agent = ExpressAgentEnum.出口易;
                                    detail.Express = request.Express;
                                    detail.OrderSign = shiporder.OrderSign;
                                    this.SaveTransactionDetail(detail);
                                }


                                break;
                        }
                        #endregion
                        break;

                    case "三态速递"://sfc

                        SFCAPI sfc = new SFCAPI();
                        #region 三态速递
                        switch (type)
                        {
                            case "Outbound":
                                shiporder = new ShipOrder();
                                break;

                            case "DirectExpress":
                                shiporder = sfc.AddExpressOrder(request, detail);

                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    detail.Agent = ExpressAgentEnum.三态速递;
                                    detail.Express = request.Express;
                                    detail.OrderSign = shiporder.OrderSign;
                                    this.SaveTransactionDetail(detail);
                                }
                                break;
                        }
                        #endregion
                        break;
                }
            }
            else
            {
                switch (agent)
                {
                    case "出口易"://ck1
                        CK1BFE ck1 = new CK1BFE();
                        
                        break;

                    case "三态速递"://sfc

                        SFCAPI sfc = new SFCAPI();
                        
                        break;
                }
            }

            return shiporder;
        }

        public string DeleteDispatchOrder(int id)
        {
            TransactionDetail detail = this.GetTransactionDetail(id);
            string agent = detail.Agent.ToString();
            string message = "";
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    message = ck1.DeleteExpressOrder(detail);
                    break;

                case "三态速递"://ck1
                    SFCAPI sfc = new SFCAPI();
                    message = sfc.DeleteExpressOrder(detail);
                    break;
            }
            
            return message;
        }
        public string SubmitDispatchOrder(int id)
        {
            TransactionDetail detail = this.GetTransactionDetail(id);
            string agent = detail.Agent.ToString();          
            string message = "";
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                     message= ck1.SubmitExpressOrder(detail);                    
                    break;

                case "三态速递"://ck1
                    SFCAPI sfc = new SFCAPI();
                    message = sfc.SubmitExpressOrder(detail);
                    break;
            }           
            return message;
        }

        #endregion

        #region paymenttransaction
        public ShipOrder AddDispatchOrder(DispatchRequest request, Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction)
        {
           
            string agent = Enum.GetName(typeof(ExpressAgentEnum), int.Parse(request.Agent));
            string type = Enum.GetName(typeof(ExpressTypeEnum), int.Parse(request.Type));
            ShipOrder shiporder = new ShipOrder();
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    #region 出口易
                    switch (type)
                    {
                        case "Outbound":
                            shiporder = ck1.AddOutboundOrder(request, paymenttransaction);

                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                paymenttransaction.Agent = ExpressAgentEnum.出口易;
                                paymenttransaction.Express = request.Express;
                                paymenttransaction.OrderSign = shiporder.OrderSign;
                                this.SavePaymentTransaction(paymenttransaction);
                            }
                            break;

                        case "DirectExpress":

                            shiporder = ck1.AddExpressOrder(request, paymenttransaction);
                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                paymenttransaction.Agent = ExpressAgentEnum.出口易;
                                paymenttransaction.Express = request.Express;
                                paymenttransaction.OrderSign = shiporder.OrderSign;
                                this.SavePaymentTransaction(paymenttransaction);
                            }


                            break;
                    }
                    #endregion
                    break;

                case "三态速递"://sfc

                    SFCAPI sfc = new SFCAPI();
                    #region 三态速递
                    switch (type)
                    {
                        case "Outbound":
                            shiporder = new ShipOrder();
                            break;

                        case "DirectExpress":
                            shiporder = sfc.AddExpressOrder(request, paymenttransaction);

                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                paymenttransaction.Agent = ExpressAgentEnum.三态速递;
                                paymenttransaction.Express = request.Express;
                                paymenttransaction.OrderSign = shiporder.OrderSign;
                                this.SavePaymentTransaction(paymenttransaction);
                            }
                            break;
                    }
                    #endregion
                    break;
            }
            return shiporder;
        }

        public ShipOrder SaveDispatchOrder(DispatchRequest request, Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction)
        {
           
            string agent = Enum.GetName(typeof(ExpressAgentEnum), int.Parse(request.Agent));
            string type = Enum.GetName(typeof(ExpressTypeEnum), int.Parse(request.Type));
            ShipOrder shiporder = new ShipOrder();

            if (string.IsNullOrEmpty(paymenttransaction.OrderSign))
            {
                switch (agent)
                {
                    case "出口易"://ck1
                        CK1BFE ck1 = new CK1BFE();
                        #region 出口易
                        switch (type)
                        {
                            case "Outbound":
                                shiporder = ck1.AddOutboundOrder(request, paymenttransaction);

                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    paymenttransaction.Agent = ExpressAgentEnum.出口易;
                                    paymenttransaction.Express = request.Express;
                                    paymenttransaction.OrderSign = shiporder.OrderSign;
                                    this.SavePaymentTransaction(paymenttransaction);
                                }
                                break;

                            case "DirectExpress":

                                shiporder = ck1.AddExpressOrder(request, paymenttransaction);
                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    paymenttransaction.Agent = ExpressAgentEnum.出口易;
                                    paymenttransaction.Express = request.Express;
                                    paymenttransaction.OrderSign = shiporder.OrderSign;
                                    this.SavePaymentTransaction(paymenttransaction);
                                }


                                break;
                        }
                        #endregion
                        break;

                    case "三态速递"://sfc

                        SFCAPI sfc = new SFCAPI();
                        #region 三态速递
                        switch (type)
                        {
                            case "Outbound":
                                shiporder = new ShipOrder();
                                break;

                            case "DirectExpress":
                                shiporder = sfc.AddExpressOrder(request, paymenttransaction);

                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    paymenttransaction.Agent = ExpressAgentEnum.三态速递;
                                    paymenttransaction.Express = request.Express;
                                    paymenttransaction.OrderSign = shiporder.OrderSign;
                                    this.SavePaymentTransaction(paymenttransaction);
                                }
                                break;
                        }
                        #endregion
                        break;
                }
            }
            else
            {
                switch (agent)
                {
                    case "出口易"://ck1
                        CK1BFE ck1 = new CK1BFE();

                        break;

                    case "三态速递"://sfc

                        SFCAPI sfc = new SFCAPI();

                        break;
                }
            }

            return shiporder;
        }

        public string DeleteDispatchOrder(Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction)
        {

            string agent = paymenttransaction.Agent.ToString();
            string message = "";
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    message = ck1.DeleteExpressOrder(paymenttransaction);
                    break;

                case "三态速递"://ck1
                    SFCAPI sfc = new SFCAPI();
                    message = sfc.DeleteExpressOrder(paymenttransaction);
                    break;
            }

            return message;
        }

        public string SubmitDispatchOrder(Evisou.Ims.Contract.Model.PayPal.PaymentTransactionType paymenttransaction)
        {

            string agent = paymenttransaction.Agent.ToString();
            string message = "";
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    message = ck1.SubmitExpressOrder(paymenttransaction);
                    break;

                case "三态速递"://ck1
                    SFCAPI sfc = new SFCAPI();
                    message = sfc.SubmitExpressOrder(paymenttransaction);
                   
                    break;
            }
            return message;
        }
        #endregion

        #region paypaltransaction
        public ShipOrder AddDispatchOrder(DispatchRequest request, PayPalTransaction paypaltransaction)
        {
            string agent = Enum.GetName(typeof(ExpressAgentEnum), int.Parse(request.Agent));
            string type = Enum.GetName(typeof(ExpressTypeEnum), int.Parse(request.Type));
            ShipOrder shiporder = new ShipOrder();
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    #region 出口易
                    switch (type)
                    {
                        case "Outbound":
                            shiporder = ck1.AddOutboundOrder(request, paypaltransaction);

                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                paypaltransaction.Agent = ExpressAgentEnum.出口易;
                                paypaltransaction.Express = request.Express;
                                paypaltransaction.OrderSign = shiporder.OrderSign;
                                this.SavePayPalTransaction(paypaltransaction);
                            }
                            break;

                        case "DirectExpress":

                            shiporder = ck1.AddExpressOrder(request, paypaltransaction);
                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                paypaltransaction.Agent = ExpressAgentEnum.出口易;
                                paypaltransaction.Express = request.Express;
                                paypaltransaction.OrderSign = shiporder.OrderSign;
                                this.SavePayPalTransaction(paypaltransaction);
                            }


                            break;
                    }
                    #endregion
                    break;

                case "三态速递"://sfc

                    SFCAPI sfc = new SFCAPI();
                    #region 三态速递
                    switch (type)
                    {
                        case "Outbound":
                            shiporder = new ShipOrder();
                            break;

                        case "DirectExpress":
                            shiporder = sfc.AddExpressOrder(request, paypaltransaction);

                            if (!string.IsNullOrEmpty(shiporder.OrderSign))
                            {
                                paypaltransaction.Agent = ExpressAgentEnum.三态速递;
                                paypaltransaction.Express = request.Express;
                                paypaltransaction.OrderSign = shiporder.OrderSign;
                                this.SavePayPalTransaction(paypaltransaction);
                            }
                            break;
                    }
                    #endregion
                    break;
            }
            return shiporder;
        }

        public ShipOrder SaveDispatchOrder(DispatchRequest request, PayPalTransaction paypaltransaction)
        {

            string agent = Enum.GetName(typeof(ExpressAgentEnum), int.Parse(request.Agent));
            string type = Enum.GetName(typeof(ExpressTypeEnum), int.Parse(request.Type));
            ShipOrder shiporder = new ShipOrder();

            if (string.IsNullOrEmpty(paypaltransaction.OrderSign))
            {
                switch (agent)
                {
                    case "出口易"://ck1
                        CK1BFE ck1 = new CK1BFE();
                        #region 出口易
                        switch (type)
                        {
                            case "Outbound":
                                shiporder = ck1.AddOutboundOrder(request, paypaltransaction);

                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    paypaltransaction.Agent = ExpressAgentEnum.出口易;
                                    paypaltransaction.Express = request.Express;
                                    paypaltransaction.OrderSign = shiporder.OrderSign;
                                    this.SavePayPalTransaction(paypaltransaction);
                                }
                                break;

                            case "DirectExpress":

                                shiporder = ck1.AddExpressOrder(request, paypaltransaction);
                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    paypaltransaction.Agent = ExpressAgentEnum.出口易;
                                    paypaltransaction.Express = request.Express;
                                    paypaltransaction.OrderSign = shiporder.OrderSign;
                                    this.SavePayPalTransaction(paypaltransaction);
                                }


                                break;
                        }
                        #endregion
                        break;

                    case "三态速递"://sfc

                        SFCAPI sfc = new SFCAPI();
                        #region 三态速递
                        switch (type)
                        {
                            case "Outbound":
                                shiporder = new ShipOrder();
                                break;

                            case "DirectExpress":
                                shiporder = sfc.AddExpressOrder(request, paypaltransaction);

                                if (!string.IsNullOrEmpty(shiporder.OrderSign))
                                {
                                    paypaltransaction.Agent = ExpressAgentEnum.三态速递;
                                    paypaltransaction.Express = request.Express;
                                    paypaltransaction.OrderSign = shiporder.OrderSign;
                                    this.SavePayPalTransaction(paypaltransaction);
                                }
                                break;
                        }
                        #endregion
                        break;
                }
            }
            else
            {
                switch (agent)
                {
                    case "出口易"://ck1
                        CK1BFE ck1 = new CK1BFE();

                        break;

                    case "三态速递"://sfc

                        SFCAPI sfc = new SFCAPI();

                        break;
                }
            }

            return shiporder;
        }

        public string DeleteDispatchOrder(PayPalTransaction paypaltransaction)
        {

            string agent = paypaltransaction.Agent.ToString();
            string message = "";
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    message = ck1.DeleteExpressOrder(paypaltransaction);
                    break;

                case "三态速递"://ck1
                    SFCAPI sfc = new SFCAPI();
                    message = sfc.DeleteExpressOrder(paypaltransaction);
                    break;
            }

            return message;
        }

        public string SubmitDispatchOrder(PayPalTransaction paypaltransaction)
        {

            string agent = paypaltransaction.Agent.ToString();
            string message = "";
            switch (agent)
            {
                case "出口易"://ck1
                    CK1BFE ck1 = new CK1BFE();
                    message = ck1.SubmitExpressOrder(paypaltransaction);
                    break;

                case "三态速递"://ck1
                    SFCAPI sfc = new SFCAPI();
                    message = sfc.SubmitExpressOrder(paypaltransaction);

                    break;
            }
            return message;
        }

        #endregion

        #endregion
        public IEnumerable<Evisou.Core.Log.AuditLog> GetAuditLogList(AuditLogRequest request = null)
        {
            request = request ?? new AuditLogRequest();

            using (var dbContext = new LogDbContext())
            {
                IQueryable<AuditLog> auditlogs = dbContext.AuditLogs;

                if (!string.IsNullOrEmpty(request.UserName))
                {
                    auditlogs = auditlogs.Where(u => u.UserName.Contains(request.UserName));
                }

                return auditlogs.OrderByDescending(u => u.ID).ToPagedList(request.PageIndex, request.PageSize);
            }
        }

    }
}
