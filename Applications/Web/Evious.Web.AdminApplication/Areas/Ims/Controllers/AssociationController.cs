using Evious.Account.Contract;
using Evious.Ims.Contract.Model;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Association)]
    public class AssociationController : AdminControllerBase
    {
        public ActionResult Datatable(PaypalApiRequest request)
        {
            #region 自定义动作
            if (!string.IsNullOrEmpty(request.customActionType))
            {
                string[] id_Array = Request.Params.GetValues("id[]");
                List<int> ids = new List<int>();

                foreach (string i in id_Array)
                {
                    ids.Add(int.Parse(i));
                }

                switch (request.customActionType)
                {
                    case "group_action":

                        switch (request.customActionName)
                        {
                            case "delete":
                                this.ImsService.DeleteAssociation(ids);
                                break;

                        }
                        break;

                    case "delete":
                        this.ImsService.DeleteAssociation(ids);
                        break;
                }
            }
            #endregion


            var allAssociation = this.ImsService.GetAssociationList(null);

            IEnumerable<Association> filterAssociation = allAssociation;

            #region 搜索
            //if (!string.IsNullOrEmpty(request.search))
            //{
            //    var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            //}
            //else if (request.action == "filter")
            //{
            //    var NameFilter = Convert.ToString(Request["name"]).Trim().ToLower();


            //    var SkuFilter = Convert.ToString(Request["sku"]).Trim().ToLower();

            //    var PackingWeightFilter = Convert.ToString(Request["packingweight"]).Trim().ToLower();

            //    var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

            //    if (isNameSearchable)
            //    {
            //        filterProduct = filterProduct.Where(c => c.Name.ToLower().Contains(NameFilter));
            //    }

            //    var isSkuSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());


            //    if (isSkuSearchable)
            //    {
            //        filterProduct = filterProduct.Where(c => c.Sku.ToLower().Contains(SkuFilter));
            //    }

            //    var isPackingWeightSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

            //    if (isPackingWeightSearchable && !string.IsNullOrEmpty(PackingWeightFilter))
            //    {
            //        filterProduct = filterProduct.Where(c => c.PackWeight == PackingWeightFilter.ToInt());
            //    }

            //}
            //else if (request.action == "filter_cancel")
            //{
            //    filterProduct = allProduct;
            //}
            //else
            //{
            //    filterProduct = allProduct;
            //}
            #endregion

            #region 排序
            //var isPPAccountSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            //var isApiUserNameSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            //var isIsActiveSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);

            //var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            //Func<PaypalApi, string> orderingFunction = (c =>
            //                                   sortColumnIndex == 1 && isPPAccountSortable ? c.PPAccount :
            //                                   sortColumnIndex == 2 && isApiUserNameSortable ? c.ApiUserName :
            //                                   sortColumnIndex == 3 && isIsActiveSortable ? c.IsActive.ToString() :
            //                                    "");

            //var sortDirection = Request["order[0][dir]"]; // asc or desc

            //if (sortDirection == "asc")
            //{
            //    filterPaypalApi = filterPaypalApi.OrderBy(orderingFunction);
            //}

            //if (sortDirection == "desc")
            //{
            //    filterPaypalApi = filterPaypalApi.OrderByDescending(orderingFunction);
            //}

            #endregion


            var displayedAssociation = filterAssociation.Skip(request.start).Take(request.length);
            var result = from c in displayedAssociation
                         select new[] {  
                                            Convert.ToString(c.ID)    
                                            ,c.ItemNumber
                                            ,c.ItemTitle//c.OrderTime.ToCnDataString()
                                            , c.PaypalApi.PPAccount//c.TransactionID                                           
                                            , c.Product.Sku.ToString() 
                                            ,c.SellingPlace
                                            ,c.StorePlace
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allAssociation.Count(),//alltransactions.Count(),
                recordsFiltered = filterAssociation.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Ims/Association/
        public ActionResult Index(AssociationRequest request)
        {
            var result = this.ImsService.GetAssociationList(request);
            return View(result);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public ActionResult Create()
        {
            var paypalapiList = this.ImsService.GetPaypalApiList(new PaypalApiRequest { IsActive = true });
            this.ViewBag.PaypalApiID = new SelectList(paypalapiList, "ID", "PPAccount");

            var productlist = this.ImsService.GetProductList();
            this.ViewBag.ProductID = new SelectList(productlist, "ID", "Sku");


            var query = from d in this.ImsService.DistinctPaymentItemInfo()
                        group new { d.Name, d.Number } by d.Number into ItemGp
                        select ItemGp.FirstOrDefault();
            this.ViewBag.ItemNumber = new SelectList(query, "Number", "Name");


            this.ViewBag.SellingPlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName");
            this.ViewBag.StorePlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName");
            var model = new Association();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Association() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            bool flag=this.TryUpdateModel<Association>(model);
            if (Convert.ToString(collection["name"]) == "association")
            {
                model.ProductID = Convert.ToInt16(collection["value[Sku]"]);
                model.PaypalApiID = Convert.ToInt16(collection["value[PaypalApiID]"]);
                model.ItemNumber = Convert.ToString(collection["value[ItemNumber]"]);
                model.ItemTitle = Convert.ToString(collection["value[ItemTitle]"]);
                model.SellingPlace = Convert.ToString(collection["value[SellingPlace]"]);
                model.StorePlace = Convert.ToString(collection["value[StorePlace]"]);
                this.ImsService.SaveAssociation(model);
            }
            else if (flag)
            {
                this.ImsService.SaveAssociation(model);
            }
            

            
           
            return this.RefreshParent();
        }


        public ActionResult Edit(int id)
        {
            var model = this.ImsService.GetAssociation(id);

            var paypalapiList = this.ImsService.GetPaypalApiList();
            this.ViewBag.PaypalApiID = new SelectList(paypalapiList, "ID", "PPAccount", model.PaypalApiID);

            var productlist = this.ImsService.GetProductList();
            this.ViewBag.ProductID = new SelectList(productlist, "ID", "Sku", model.ProductID);


            //var query = from d in this.ImsService.DistinctTransactionItem()
            //            group new { d.ItemTitle } by d.ItemTitle into ItemGp
            //            select ItemGp.FirstOrDefault();
            //this.ViewBag.ItemTitle = new SelectList(query, "ItemTitle", "ItemTitle", model.ItemTitle);
            var query = from d in this.ImsService.DistinctPaymentItemInfo()
                        group new { d.Name, d.Number } by d.Number into ItemGp
                        select ItemGp.FirstOrDefault();

            this.ViewBag.ItemNumber = new SelectList(query, "Number", "Name", model.ItemNumber);

            this.ViewBag.SellingPlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName", model.SellingPlace);
            this.ViewBag.StorePlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName", model.StorePlace);
            
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.ImsService.GetAssociation(id);
            this.TryUpdateModel<Association>(model);

            this.ImsService.SaveAssociation(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteAssociation(ids);
            return RedirectToAction("Index");
        }

        public ActionResult SkuAndPayPalJson()
        {
            var paypalapiList = this.ImsService.GetPaypalApiList(new PaypalApiRequest { IsActive=true });

            var productlist = this.ImsService.GetProductList();

            var countrylit = this.ImsService.GetCountryNameAndCode();
            return Json(new
            {
                
                data = new {
                    paypal=paypalapiList,
                    sku=productlist,
                    country = countrylit
                }
            },
                            JsonRequestBehavior.AllowGet);
        }
    }
}