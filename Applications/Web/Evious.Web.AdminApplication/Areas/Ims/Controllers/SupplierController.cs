using Evious.Account.Contract;
using Evious.Framework.Utility;
using Evious.Ims.Contract.Model;
using Evious.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Supplier)]
    public class SupplierController : AdminControllerBase
    {


        public JsonResult Datatable(SupplierRequest request)
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
                                this.ImsService.DeleteSupplier(ids);
                                break;

                        }
                        break;

                    case "delete":
                        this.ImsService.DeleteSupplier(ids);
                        break;
                }
            }
            #endregion

            var allSuppliers = this.ImsService.GetSupplierList(null);

            IEnumerable<Supplier> filterSuppliers = allSuppliers;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).ToLower().Trim();
                var PlatFormFilter = Convert.ToString(Request["platform"]).ToLower().Trim();
                var URLFilter = Convert.ToString(Request["url"]).ToLower().Trim();
                var CSFilter = Convert.ToString(Request["cs"]).ToLower().Trim();
                var CSPhoneFilter = Convert.ToString(Request["csphone"]).ToLower().Trim();
                var ContactFilter = Convert.ToString(Request["contact"]).ToLower().Trim();
                var ContactPhoneFilter = Convert.ToString(Request["contactphone"]).ToLower().Trim();

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
                
                var isPlatFormSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());


                if (isPlatFormSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.Platform.ToLower().Contains(PlatFormFilter));
                }

                var isURLSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                if (isURLSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.URL.ToLower().Contains(URLFilter));                   
                }

                var isCSSearchable = Convert.ToBoolean(Request["columns[4][searchable]"].ToString());

                if (isCSSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.CS.ToLower().Contains(CSFilter));
                }

                var isCSPhoneSearchable = Convert.ToBoolean(Request["columns[5][searchable]"].ToString());

                if (isCSPhoneSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.CSPhone.ToLower().Contains(CSPhoneFilter));
                }

                var isContactSearchable = Convert.ToBoolean(Request["columns[6][searchable]"].ToString());

                if (isContactSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.Contact.ToLower().Contains(ContactFilter));
                }
                var isContactPhoneSearchable = Convert.ToBoolean(Request["columns[7][searchable]"].ToString());

                if (isContactPhoneSearchable)
                {
                    filterSuppliers = filterSuppliers.Where(c => c.ContactPhone.ToLower().Contains(ContactPhoneFilter));
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterSuppliers = allSuppliers;
            }
            else
            {
                filterSuppliers = allSuppliers;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isPlatFormSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isUrlSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);
            var isCsSortable = Convert.ToBoolean(Request["columns[4][orderable]"]);
            var isCsPhoneSortable = Convert.ToBoolean(Request["columns[5][orderable]"]);
            var isContactSortable = Convert.ToBoolean(Request["columns[6][orderable]"]);
            var isContactPhoneSortable = Convert.ToBoolean(Request["columns[7][orderable]"]);


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Supplier, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                sortColumnIndex == 2 && isNameSortable ? c.Platform :
                                                sortColumnIndex == 3 && isNameSortable ? c.URL :
                                                sortColumnIndex == 4 && isNameSortable ? c.CS :
                                                sortColumnIndex == 5 && isNameSortable ? c.CSPhone.ToString() :
                                                sortColumnIndex == 6 && isNameSortable ? c.Contact :
                                                sortColumnIndex == 7 && isNameSortable ? c.ContactPhone.ToString() :
                                                
                                                "");
            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterSuppliers = filterSuppliers.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterSuppliers = filterSuppliers.OrderByDescending(orderingFunction);
            }

            #endregion

            var displayedSuppliers = filterSuppliers.Skip(request.start).Take(request.length);
            var result = from c in displayedSuppliers
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.Platform                                            
                                            , c.URL//StringUtil.CutString(StringUtil.RemoveHtml(c.Content), 100)
                                            ,c.CS
                                            ,c.CSPhone
                                            ,c.Contact
                                            ,c.ContactPhone
                                            ,StringUtil.CutString(c.Address,100)
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allSuppliers.Count(),
                recordsFiltered = filterSuppliers.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Ims/Supplier/
        public ActionResult Index(SupplierRequest request)
        {
            var result = this.ImsService.GetSupplierList(request);
            return View(result);
        }

        public ActionResult Create()
        {
            /*var channelList = this.CmsService.GetChannelList(new ChannelRequest() { IsActive = true });
            this.ViewBag.ChannelId = new SelectList(channelList, "ID", "Name");
            this.ViewBag.Tags = this.CmsService.GetTagList(new TagRequest() { Top = 20, Orderby = Orderby.Hits });*/
            ViewBag.Products = this.ImsService.GetProductList(null);
            var model = new Supplier();
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Supplier() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<Supplier>(model);

            this.ImsService.SaveSupplier(model);

            return this.RefreshParent();
        }


        public ActionResult Edit(int id)
        {
            ViewBag.Products = this.ImsService.GetProductList(null);
            var model = this.ImsService.GetSupplier(id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (collection["SelectedProduct"] == null)
            {
                collection.Add("SelectedProduct", null);
            }
            //collection["SelectedProduct"] == null ? collection.Add("SelectedProduct", null) : collection;
            var model = this.ImsService.GetSupplier(id);
            this.TryUpdateModel<Supplier>(model, collection);

            this.ImsService.SaveSupplier(model);

            return this.RefreshParent();
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteSupplier(ids);
            return RedirectToAction("Index");
        }
    }
}