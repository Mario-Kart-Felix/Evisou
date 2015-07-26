using Evisou.Account.Contract;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using Evisou.Ims.Contract.Model;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Ims.Controllers
{
    [Permission(EnumBusinessPermission.ImsManage_Product)]
    public class ProductController : AdminControllerBase
    {
        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

           // response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        private string HttpGet(string postDataStr)
        {

            string requestUriString ="http://"+ System.Web.HttpContext.Current.Request.Url.Host +":"+ System.Web.HttpContext.Current.Request.Url.Port+ System.Web.HttpContext.Current.Request.ApplicationPath + "UploadHandler.ashx" + (postDataStr == "" ? "" : "?") + postDataStr;//"http://localhost:46088/UploadHandler.ashx" + (postDataStr == "" ? "" : "?") + postDataStr;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.Method = "DELETE";           
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        private void DeleteImageFile(string[] id_Array)
        {
            foreach (string i in id_Array)
            {
                var product = this.ImsService.GetProduct(int.Parse(i));
                if (product.Images != null)
                {
                    foreach (Image image in product.Images)
                    {
                        Image img = this.ImsService.GetImage(image.PictureURL);
                        if (img != null)
                        {
                            this.HttpGet("f=" + image.PictureURL);
                        }
                    }
                }
            }
        }
        private void DeleteImageFileIO(string[] id_Array)
        {
            foreach (string i in id_Array)
            {
              
                var product = this.ImsService.GetProduct(int.Parse(i));
                if (product.Images != null)
                {
                    foreach (Image image in product.Images)
                    {
                        Image img = this.ImsService.GetImage(image.PictureURL);
                        if (img != null)
                        {
                            string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(image.PictureURL));

                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            string thumNailfilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(image.PictureURL.Replace(".", "_s.")));

                            if (System.IO.File.Exists(thumNailfilePath))
                            {
                                System.IO.File.Delete(thumNailfilePath);
                            }
                        }
                    }
                }
            }
        }

        private void DeleteImageFileIO( List<int> ids)
        {
            var request = new ProductRequest { IDs = ids };
            var products = this.ImsService.GetProductList(request);
            foreach (Product product in products)
            {
                if (!string.IsNullOrEmpty(product.CoverPicture))
                {
                    string CoverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(product.CoverPicture));

                    if (System.IO.File.Exists(CoverPath))
                    {
                        System.IO.File.Delete(CoverPath);
                    }

                    string thumNailCoverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(product.CoverPicture.Replace(".", "_s.")));

                    if (System.IO.File.Exists(thumNailCoverPath))
                    {
                        System.IO.File.Delete(thumNailCoverPath);
                    }
                }

               if (product.Images != null)
                {
                    foreach (Image image in product.Images)
                    {
                        string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(image.PictureURL));

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        string thumNailfilePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(image.PictureURL.Replace(".", "_s.")));

                        if (System.IO.File.Exists(thumNailfilePath))
                        {
                            System.IO.File.Delete(thumNailfilePath);
                        }
                    }
                }
            }
        }


        public string AjaxCheckForm(ProductRequest request)
        {
            
            var products = this.ImsService.GetProductList(request);

            return products.Count<Product>() == 0 ? "true" : "false";
           
        }

        public ActionResult Datatable(ProductRequest request)
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
                               // DeleteImageFile(id_Array);
                                DeleteImageFileIO(ids);
                                this.ImsService.DeleteProduct(ids);
                                break;

                        }
                        break;

                    case "delete":
                       // DeleteImageFile(id_Array);
                        DeleteImageFileIO(ids);
                        this.ImsService.DeleteProduct(ids);
                        break;
                }
            }
            #endregion


            var allProduct = this.ImsService.GetProductList(null);

            IEnumerable<Product> filterProduct = allProduct;

            #region 搜索
            if (!string.IsNullOrEmpty(request.search))
            {
                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"]);

            }
            else if (request.action == "filter")
            {
                var NameFilter = Convert.ToString(Request["name"]).Trim().ToLower();
               

                var SkuFilter = Convert.ToString(Request["sku"]).Trim().ToLower();

                var PackingWeightFilter = Convert.ToString(Request["packingweight"]).Trim().ToLower();

                var isNameSearchable = Convert.ToBoolean(Request["columns[1][searchable]"].ToString());

                if (isNameSearchable)
                {
                    filterProduct = filterProduct.Where(c => c.Name.ToLower().Contains(NameFilter));
                }
               
                 var isSkuSearchable = Convert.ToBoolean(Request["columns[2][searchable]"].ToString());


                 if (isSkuSearchable)
                {
                    filterProduct = filterProduct.Where(c => c.Sku.ToLower().Contains(SkuFilter));
                }

                 var isPackingWeightSearchable = Convert.ToBoolean(Request["columns[3][searchable]"].ToString());

                 if (isPackingWeightSearchable && !string.IsNullOrEmpty(PackingWeightFilter))
                {
                   filterProduct = filterProduct.Where(c => c.PackWeight == PackingWeightFilter.ToInt());
                }

            }
            else if (request.action == "filter_cancel")
            {
                filterProduct = allProduct;
            }
            else
            {
                filterProduct = allProduct;
            }
            #endregion

            #region 排序
            var isNameSortable = Convert.ToBoolean(Request["columns[1][orderable]"]);
            var isSkuSortable = Convert.ToBoolean(Request["columns[2][orderable]"]);
            var isWeightSortable = Convert.ToBoolean(Request["columns[3][orderable]"]);


            var sortColumnIndex = Convert.ToInt32(Request["order[0][column]"]);

            Func<Product, string> orderingFunction = (c =>
                                                sortColumnIndex == 1 && isNameSortable ? c.Name :
                                                sortColumnIndex == 2 && isSkuSortable ? c.Sku :
                                                sortColumnIndex==3&&isWeightSortable?c.Weight.ToString():
                                                "");

            var sortDirection = Request["order[0][dir]"]; // asc or desc

            if (sortDirection == "asc")
            {
                filterProduct = filterProduct.OrderBy(orderingFunction);
            }

            if (sortDirection == "desc")
            {
                filterProduct = filterProduct.OrderByDescending(orderingFunction);
            }

            #endregion
            var displayedProduct = filterProduct.Skip(request.start).Take(request.length);
            var result = from c in displayedProduct
                         select new[] {  
                                            Convert.ToString(c.ID)                                 
                                            , c.Name
                                            , c.Sku
                                            , c.Weight.ToString()
                                            , c.NetSize.Length.ToString()+"*"+c.NetSize.Width.ToString()+"*"+c.NetSize.Height.ToString()
                                            , c.PackWeight.ToString()
                                            , c.PackingSize.Length.ToString()+"*"+c.PackingSize.Width.ToString()+"*"+c.PackingSize.Height.ToString()                                                                                   
                                            ,Convert.ToString(c.ID)
                         
                         };
            return Json(new
            {
                draw = request.draw,//param.sEcho,
                recordsTotal = allProduct.Count(),//alltransactions.Count(),
                recordsFiltered = filterProduct.Count(),
                data = result
            },
                             JsonRequestBehavior.AllowGet);

        }
        //
        // GET: /Pms/Product/
        public ActionResult Index(ProductRequest request)
        {
            var result = this.ImsService.GetProductList(request);
            return View(result);

        }

        public ActionResult Create()
        {
            var model = new Product();
            return PartialView("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Product() { UserId = this.UserContext.LoginInfo.UserID, UserName = this.UserContext.LoginInfo.LoginName };
            this.TryUpdateModel<Product>(model);

            try
            {
                if (ModelState.IsValid)
                {

                    this.ImsService.SaveProduct(model);

                }
                string url = Url.Action("Index", "Product");
                return Json(new { success = true, url = url });

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);              

                return PartialView("Edit", model);
            }

           

           
        }

        public ActionResult Edit(int id)
        {

            var model = this.ImsService.GetProduct(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = this.ImsService.GetProduct(id);            
            this.TryUpdateModel<Product>(model);
            try
            {
                if (ModelState.IsValid)
                {

                    this.ImsService.SaveProduct(model);

                }
                string url = Url.Action("Index", "Product");
                return Json(new { success = true, url = url });

            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);

                return PartialView("Edit", model);
            }
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            this.ImsService.DeleteProduct(ids);
            return RedirectToAction("Index");
        }


        //public ActionResult Image(string Sku)
        //{ 
        
        //}
    }
}