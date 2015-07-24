using Evious.Core.Upload;
using Evious.Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Evious.Web;


namespace Evious.Web.AdminApplication
{
    /// <summary>
    /// FileUpload 的摘要说明
    /// </summary>
    public class FileUpload : Evious.Core.Upload.UploadHandler
    {
        //返回给UploadyFile的Json结果
        public override string GetResult(string localFileName, string uploadFilePath, string err)
        {
            var result = new
            {
                msg = new
                {
                    localname = localFileName,
                    url = uploadFilePath
                    .Substring(uploadFilePath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase))
                    .Replace("\\", "/")
                },
                err = err
            };

            return JsonConvert.SerializeObject(result);
            //List<object> Objects = new List<object>();
            //var filename=GetUrlFileName(uploadFilePath.Substring(uploadFilePath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\\", "/"));
            //Objects.Add(new
            //    {
            //        deleteType="DELETE",
            //        deleteUrl = HttpContext.Current.Request.Url+ "?filename=" + filename,
            //        name =filename ,
            //        url=uploadFilePath.Substring(uploadFilePath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\\", "/")
            //    });
            //var result = new
            //{
            //    files = Objects
            //};
            //return JsonConvert.SerializeObject(result);
        }

        //即时生成如201323023_s.jpg的缩略图
        public override void OnUploaded(HttpContext context, string filePath)
        {
            var ext = filePath.Substring(filePath.LastIndexOf('.') + 1).ToLower();
            if (!ImageExt.Contains(ext))
                return;

            if (string.IsNullOrEmpty(context.Request["thumbs"]))
            {
                this.MakeThumbnail(filePath, "s", context.Request["thumbwidth"].ToInt(85), context.Request["thumbheight"].ToInt(85), string.IsNullOrEmpty(context.Request["mode"]) ? "H" : context.Request["mode"]);
            }
            else
            {
                var thumbs = context.Request["thumbs"];
                foreach (var thumb in thumbs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var thumbparts = thumb.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    this.MakeThumbnail(filePath, thumbparts[0], thumbparts[1].ToInt(), thumbparts[2].ToInt(), thumbparts[3]);
                }
            }
        }

        private void MakeThumbnail(string filePath, string suffix, int width, int height, string mode)
        {
            string fileExt = filePath.Substring(filePath.LastIndexOf('.'));
            string fileHead = filePath.Substring(0, filePath.LastIndexOf('.'));

            var thumbPath = string.Format("{0}_{1}{2}", fileHead, suffix, fileExt); ;
            ThumbnailHelper.MakeThumbnail(filePath, thumbPath, width, height, mode, false);
        }


        private static string GetUrlFileName(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }  
    }
}