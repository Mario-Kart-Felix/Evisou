using Evious.Account.Contract;
using Evious.Ims.Contract.Enum;
using Evious.Ims.Contract.Model;
using Evious.Web.Admin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.Admin.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Association)]
    public class AssociationController : AdminControllerBase
    {
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
            var paypalapiList = this.ImsService.GetPaypalApiList();
            this.ViewBag.PaypalApiID = new SelectList(paypalapiList, "ID", "PPAccount");

            var productlist = this.ImsService.GetProductList();
            this.ViewBag.ProductID = new SelectList(productlist, "ID", "Sku");


            var query = from d in this.ImsService.DistinctTransactionItem()
                        group new { d.ItemTitle } by d.ItemTitle into ItemGp
                        select ItemGp.FirstOrDefault();
            this.ViewBag.ItemTitle = new SelectList(query, "ItemTitle", "ItemTitle");


            this.ViewBag.SellingPlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName");
            this.ViewBag.StorePlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName");
            var model = new Association();            
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Association() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<Association>(model);
            this.ImsService.SaveAssociation(model);
            return this.RefreshParent();
        }


        public ActionResult Edit(int id)
        {
            var model = this.ImsService.GetAssociation(id);

            var paypalapiList = this.ImsService.GetPaypalApiList();
            this.ViewBag.PaypalApiID = new SelectList(paypalapiList, "ID", "PPAccount",model.PaypalApiID);

            var productlist = this.ImsService.GetProductList();
            this.ViewBag.ProductID = new SelectList(productlist, "ID", "Sku",model.ProductID);


            var query = from d in this.ImsService.DistinctTransactionItem()
                        group new { d.ItemTitle } by d.ItemTitle into ItemGp
                        select ItemGp.FirstOrDefault();
            this.ViewBag.ItemTitle = new SelectList(query, "ItemTitle", "ItemTitle", model.ItemTitle);


            this.ViewBag.SellingPlace = new SelectList(this.ImsService.GetCountryNameAndCode(), "ShipToCountryCode", "ShipToCountryName",model.SellingPlace);
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
	}
}